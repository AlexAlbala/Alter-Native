using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using USAL;

namespace RTDP
{
    class IrViMapper
    {
        // NOTE: In the system, X points up in the image, Y to the right
        //       In IrViMapper, X points to the right, Y down in the image

        CAMProperties irCamera; // internal and external parameters of the IR camera
        CAMProperties viCamera; // internal and external parameters of the VI camera

        int M_ir;  // irCamera.imageHeight;
        int N_ir;  // irCamera.imageWidth;
        int M_vi;  // viCamera.imageHeight;
        int N_vi;  // viCamera.imageWidth;


        Matrix3 AffineMatrixIrVi;
        Matrix3 AffineMatrixViIr;

        Rectangle viLockRectangle; // VI rectangle corresponding to the IR image

        public IrViMapper(CAMProperties irCam, CAMProperties viCam)
        {
            // Camera properties remain constant during the flight
            irCamera = irCam;
            viCamera = viCam;

            // IR and VI image size
            M_ir = irCamera.imageHeight;
            N_ir = irCamera.imageWidth;
            M_vi = viCamera.imageHeight;
            N_vi = viCamera.imageWidth;
        }

        public void Init(Metadata mt)
        {
            double cosYaw = Math.Cos(mt.uavTelemetry.uavAngles.Yaw);
            double sinYaw = Math.Sin(mt.uavTelemetry.uavAngles.Yaw);
            double speed_x =  mt.uavTelemetry.uavSpeed.North * cosYaw + mt.uavTelemetry.uavSpeed.East * sinYaw; // to the nose
            double speed_y = -mt.uavTelemetry.uavSpeed.North * sinYaw + mt.uavTelemetry.uavSpeed.East * cosYaw; // to the right wing
            double delay_ir = mt.camDelay.irDelay;
            double delay_vi = mt.camDelay.viAverage;

            double height = mt.AGL - (irCamera.camPosition.z + viCamera.camPosition.z) / 2;
            double tilt_ir = mt.uavTelemetry.uavAngles.Pitch + irCamera.camAngles.Pitch;
            double tilt_vi = mt.uavTelemetry.uavAngles.Pitch + viCamera.camAngles.Pitch;

            // Scale factor and offsets between IR and VI for three IR pixels
            double S_ir_vi = (irCamera.pixelPitch * viCamera.focalLength) / (irCamera.focalLength * viCamera.pixelPitch);
            double ydif = ((irCamera.camPosition.y - viCamera.camPosition.y) +0.8 + speed_y * (delay_ir - delay_vi)) * viCamera.focalLength / (height * viCamera.pixelPitch);
            double xdif = ((irCamera.camPosition.x - viCamera.camPosition.x) -1.2 + speed_x * (delay_ir - delay_vi)) / height;
            double tao_ir, tao_vi, sm_ir_vi, sm_vi;

            // Top-left and top-right pixels
            tao_ir = tilt_ir + Math.Atan2((M_ir - 1) * irCamera.pixelPitch, 2 * irCamera.focalLength);
            tao_vi = Math.Atan(xdif + Math.Tan(tao_ir));
            int y_tl_vi = (int)(M_vi / 2 - viCamera.focalLength * Math.Tan(tao_vi - tilt_vi) / viCamera.pixelPitch);
            int y_tr_vi = y_tl_vi;

            sm_ir_vi = S_ir_vi * (Math.Cos(tao_vi) * Math.Cos(tao_ir - tilt_ir)) / (Math.Cos(tao_ir) * Math.Cos(tao_vi - tilt_vi));
            sm_vi = Math.Cos(tao_vi) / Math.Cos(tao_vi - tilt_vi);
            int x_tl_vi = (int)((N_vi - sm_ir_vi * N_ir) / 2 + ydif * sm_vi);
            int x_tr_vi = (int)(x_tl_vi + N_ir * sm_ir_vi);

            // Bottom-left and bottom-right pixels
            tao_ir = tilt_ir - Math.Atan2((M_ir - 1) * irCamera.pixelPitch, 2 * irCamera.focalLength);
            tao_vi = Math.Atan(xdif + Math.Tan(tao_ir));
            int y_bl_vi = (int)(M_vi / 2 - viCamera.focalLength * Math.Tan(tao_vi - tilt_vi) / viCamera.pixelPitch);
            int y_br_vi = y_bl_vi;

            sm_ir_vi = S_ir_vi * (Math.Cos(tao_vi) * Math.Cos(tao_ir - tilt_ir)) / (Math.Cos(tao_ir) * Math.Cos(tao_vi - tilt_vi));
            sm_vi = Math.Cos(tao_vi) / Math.Cos(tao_vi - tilt_vi);
            int x_bl_vi = (int)((N_vi - sm_ir_vi * N_ir) / 2 + ydif * sm_vi);
            int x_br_vi = (int)(x_bl_vi + N_ir * sm_ir_vi);

            // Compute affine matrix
            this.ComputeAffineMatrix(x_tl_vi, y_tl_vi, x_tr_vi, y_tr_vi, x_bl_vi, y_bl_vi, x_br_vi, y_br_vi);

            //// Rectangle enclosing the IR image in the VI image
            int top = y_tl_vi;
            int bottom = y_bl_vi;
            int left = (x_tl_vi < x_bl_vi) ? x_tl_vi : x_bl_vi;
            int right = (x_tr_vi > x_br_vi) ? x_tr_vi : x_br_vi;

            if (top < 0) top = 0;
            if (top > M_vi - 1) top = M_vi - 1;
            if (left < 0) left = 0;
            if (left > N_vi - 1) left = N_vi - 1;
            if (bottom < top) bottom = top;
            if (bottom > M_vi - 1) bottom = M_vi - 1;
            if (right < left) right = left;
            if (right > N_vi - 1) right = N_vi -1;
         
            viLockRectangle = new Rectangle(left, top, right - left + 1, bottom - top + 1);

        }

        // Input parameters are the vi coordinates corresponding to the IR top-left (x1, y1), 
        // top-right (x2, y2), bottom-left (x3, y3) and bottom-right (x4, y4) corners.
        //
        // We have two systems with four equations and three incognites:
        // 	x1vi = a11 * x1ir + a12 * y1ir + a13
        // 	x2vi = a11 * x2ir + a12 * y2ir + a13
        // 	x3vi = a11 * x3ir + a12 * y3ir + a13
        // 	x4vi = a11 * x4ir + a12 * y4ir + a13
        // 
        // 	y1vi = a21 * x1ir + a22 * y1ir + a23
        // 	y2vi = a21 * x2ir + a22 * y2ir + a23
        // 	y3vi = a21 * x3ir + a22 * y3ir + a23
        // 	y4vi = a21 * x4ir + a22 * y4ir + a23
        //
        // Given a system AX=B
        // The least square solution is given by: (At·A)·X = (At·B)
        //
        // For the first system
        // 	[x1vi] = [x1ir y1ir 1] 
        // 	[x2vi] = [x2ir y2ir 1] [a11]
        // 	[x3vi] = [x3ir y3ir 1]·[a12]
        // 	[x4vi] = [x4ir y4ir 1] [a13]
        //
        // The least square solution is computed by solving the system:	
        //			              [x1vi]                         [x1ir y1ir 1]
        //	[x1ir x2ir x3ir x4ir] [x2vi]   [x1ir x2ir x3ir x4ir] [x2ir y2ir 1] [a11]
        //	[y1ir y2ir y3ir y4ir]·[x3vi] = [y1ir y2ir y3ir y4ir]·[x3ir y3ir 1]·[a12]
        //  [   1    1    1    1] [x4vi]   [   1    1    1    1] [x4ir y4ir 1] [a13]

        private void ComputeAffineMatrix(int x1vi, int y1vi, int x2vi, int y2vi, int x3vi, int y3vi, int x4vi, int y4vi)
        {
            int x1ir = 0;
            int y1ir = 0;
            int x2ir = N_ir - 1;
            int y2ir = 0;
            int x3ir = 0;
            int y3ir = M_ir - 1;
            int x4ir = N_ir - 1;
            int y4ir = M_ir - 1;

            Vector3 Bx = new Vector3(x1ir * x1vi + x2ir * x2vi + x3ir * x3vi + x4ir * x4vi,
                                     y1ir * x1vi + y2ir * x2vi + y3ir * x3vi + y4ir * x4vi,
                                            x1vi + x2vi + x3vi + x4vi);

            Vector3 By = new Vector3(x1ir * y1vi + x2ir * y2vi + x3ir * y3vi + x4ir * y4vi,
                                     y1ir * y1vi + y2ir * y2vi + y3ir * y3vi + y4ir * y4vi,
                                            y1vi + y2vi + y3vi + y4vi);

            Matrix3 A = new Matrix3(x1ir * x1ir + x2ir * x2ir + x3ir * x3ir + x4ir * x4ir,
                                     x1ir * y1ir + x2ir * y2ir + x3ir * y3ir + x4ir * y4ir,
                                     x1ir + x2ir + x3ir + x4ir,
                                     y1ir * x1ir + y2ir * x2ir + y3ir * x3ir + y4ir * x4ir,
                                     y1ir * y1ir + y2ir * y2ir + y3ir * y3ir + y4ir * y4ir,
                                     y1ir + y2ir + y3ir + y4ir,
                                            x1ir + x2ir + x3ir + x4ir,
                                            y1ir + y2ir + y3ir + y4ir,
                                               1 + 1 + 1 + 1);


            double det = A.Determinant();

            if (det == 0)
            {
                // the system has not solution
                AffineMatrixIrVi = AffineMatrixViIr = null;
                return;
            }

            double a11 = (new Matrix3(Bx.x, A.m01, A.m02, Bx.y, A.m11, A.m12, Bx.z, A.m21, A.m22)).Determinant() / det;
            double a12 = (new Matrix3(A.m00, Bx.x, A.m02, A.m10, Bx.y, A.m12, A.m20, Bx.z, A.m22)).Determinant() / det;
            double a13 = (new Matrix3(A.m00, A.m01, Bx.x, A.m10, A.m11, Bx.y, A.m20, A.m21, Bx.z)).Determinant() / det;

            double a21 = (new Matrix3(By.x, A.m01, A.m02, By.y, A.m11, A.m12, By.z, A.m21, A.m22)).Determinant() / det;
            double a22 = (new Matrix3(A.m00, By.x, A.m02, A.m10, By.y, A.m12, A.m20, By.z, A.m22)).Determinant() / det;
            double a23 = (new Matrix3(A.m00, A.m01, By.x, A.m10, A.m11, By.y, A.m20, A.m21, By.z)).Determinant() / det;

            AffineMatrixIrVi = new Matrix3(a11, a12, a13, a21, a22, a23, 0, 0, 1);
            AffineMatrixViIr = AffineMatrixIrVi.Inverse();
        }

        public Rectangle ViLockRectangle
        {
            get { return viLockRectangle; }
        }

        public bool LockVIpToIRp(Point vi_pixel, ref Point ir_pixel)
        {
            ir_pixel.X = (int)(AffineMatrixViIr.m00 * (vi_pixel.X + viLockRectangle.Left) + AffineMatrixViIr.m01 * (vi_pixel.Y + viLockRectangle.Top) + AffineMatrixViIr.m02);
            ir_pixel.Y = (int)(AffineMatrixViIr.m10 * (vi_pixel.X + viLockRectangle.Left) + AffineMatrixViIr.m11 * (vi_pixel.Y + viLockRectangle.Top) + AffineMatrixViIr.m12);

            if (ir_pixel.Y == -1 || ir_pixel.Y > M_ir - 1) return false;
            if (ir_pixel.X < 0 || ir_pixel.X > N_ir - 1) return false;

            return true;
        }

        public bool IRpToLockVIp(Point ir_pixel, ref Point vi_pixel)
        {
            vi_pixel.X = (int)(AffineMatrixIrVi.m00 * ir_pixel.X + AffineMatrixIrVi.m01 * ir_pixel.Y + AffineMatrixIrVi.m02 - viLockRectangle.Left);
            vi_pixel.Y = (int)(AffineMatrixIrVi.m10 * ir_pixel.X + AffineMatrixIrVi.m11 * ir_pixel.Y + AffineMatrixIrVi.m12 - viLockRectangle.Top);

            if (vi_pixel.Y < 0 || vi_pixel.Y >= viLockRectangle.Height) return false;
            if (vi_pixel.X < 0 || vi_pixel.X >= viLockRectangle.Width) return false;

            return true;
        }

        // returns the VI rectangle of pixels in the locked VI image corresponding to a specific IR pixel     
        public bool IRpToLockVIr(Point ir_pixel, ref Rectangle vi_rect)
        {
            Point top_left_pixel = new Point();
            if (!IRpToLockVIp(ir_pixel, ref top_left_pixel)) return false;

            Point next_ir_pixel = new Point(ir_pixel.X + 1, ir_pixel.Y + 1);
            Point next_top_left_pixel = new Point();
            if (!IRpToLockVIp(next_ir_pixel, ref next_top_left_pixel)) return false;

            vi_rect = new Rectangle(top_left_pixel.X, top_left_pixel.Y, next_top_left_pixel.X - top_left_pixel.X, next_top_left_pixel.Y - top_left_pixel.Y);
            return true;
        }

        //public Rectangle IRrToLockVIr(Rectangle ir_rect)
        //{
        //    int top = Ty_ir_vi_m[ir_rect.Top] - viLockRectangle.Top;
        //    int bottom = Ty_ir_vi_m[ir_rect.Bottom] - viLockRectangle.Top - 1;
        //    int left_top = (int)(Tx_ir_vi_m[ir_rect.Top] + ir_rect.Left * S_ir_vi_m[ir_rect.Top] - viLockRectangle.Left);
        //    int left_bottom = (int)(Tx_ir_vi_m[ir_rect.Bottom] + ir_rect.Left * S_ir_vi_m[ir_rect.Bottom] - viLockRectangle.Left);
        //    int left = (left_top < left_bottom) ? left_top : left_bottom;
        //    int right_top = (int)(Tx_ir_vi_m[ir_rect.Top] + ir_rect.Left * S_ir_vi_m[ir_rect.Top] - viLockRectangle.Left);
        //    int right_bottom = (int)(Tx_ir_vi_m[ir_rect.Bottom] + ir_rect.Right * S_ir_vi_m[ir_rect.Bottom] - viLockRectangle.Left);
        //    int right = (right_top > right_bottom) ? right_top : right_bottom;

        //    if (top < viLockRectangle.Top) top = viLockRectangle.Top;
        //    if (top > viLockRectangle.Bottom) top = viLockRectangle.Bottom;
        //    if (left < viLockRectangle.Left) left = viLockRectangle.Left;
        //    if (left > viLockRectangle.Right) left = viLockRectangle.Right;
        //    if (bottom < top) bottom = top;
        //    if (bottom > viLockRectangle.Bottom) bottom = viLockRectangle.Bottom;
        //    if (right < left) right = left;
        //    if (right > viLockRectangle.Right) right = viLockRectangle.Right;

        //    return (new Rectangle(left, top, right - left + 1, bottom - top + 1));
        //}

    }
}
