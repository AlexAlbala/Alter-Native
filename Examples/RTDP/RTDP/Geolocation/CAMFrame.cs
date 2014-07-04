using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USAL;

namespace RTDP
{
    class CAMFrame
    {
        Vector3 camPosition;  // Coordinates of the optical centre in the body frame
        Angles  camAngles;    // roll-pitch-yaw (0-elevation-azimut) respect to the body frame)
                              //   yaw/azimut: 0-to the nose, 90-to the right wing
                              //   pitch/elevation: 0-vertical down, 90-horizontal 

        double scaleFactor;   // Meters/pixel relation in the image plane
        double offsetX;       // Vertical offset (m) of the upper-left corner to the center in the image plane 
        double offsetY;       // Horizontal offset (m) of the upper-left corner to the center in the image plane
        double focalLength;   // Focal length

        // Settings
        bool  zeroCamPanRoll;    // Assume camera pan and roll angles are zero
        bool  zeroCamDistance;   // Assume camera distance to the UAV center of mass is zero

        // Rotation matrices
        Matrix3 R_BODY_CAM;
        Matrix3 R_CAM_BODY;

        double cosYaw,   sinYaw;
        double cosPitch, sinPitch;
        double cosRoll,  sinRoll;

        // Properties
        public Vector3 CamBODY
        {
            get { return camPosition; }
        }

        // Constructor. CameraProperties remain constant during the mission
        public CAMFrame(CAMProperties camera)
        {
            zeroCamDistance = RTDPSettings.geolocationSettings.zeroCamDistance;
            zeroCamPanRoll = RTDPSettings.geolocationSettings.zeroCamPanRoll;

            camPosition = camera.camPosition;

            scaleFactor = camera.pixelPitch;
            offsetX = (camera.imageHeight - 1) * scaleFactor / 2;
            offsetY = (camera.imageWidth - 1) * scaleFactor / 2;
            focalLength = camera.focalLength;

            camAngles = camera.camAngles;
            cosPitch = Math.Cos(camAngles.Pitch);
            sinPitch = Math.Sin(camAngles.Pitch);
            if (zeroCamPanRoll)
            {
                cosYaw = cosRoll = 1;
                sinYaw = sinRoll = 0;
            }
            else
            {
                cosYaw = Math.Cos(camAngles.Yaw);
                sinYaw = Math.Sin(camAngles.Yaw);
                cosRoll = Math.Cos(camAngles.Roll);
                sinRoll = Math.Sin(camAngles.Roll);
            }

            R_BODY_CAM = null;
            R_CAM_BODY = null;
        }

        // This method would be used if some of the camera properties 
        // was changed during the mission (cameraangles, for example)
        public void Init()
        {
        }

        // Body to Camera system transformation
        public Vector3 BODYtoCAM(Vector3 BODY)
        {
            if (zeroCamPanRoll && zeroCamDistance)
            {
                return new Vector3 (cosPitch * BODY.x - sinPitch * BODY.z,
                                    BODY.y,
                                    sinPitch * BODY.x + cosPitch * BODY.z);
            }
            else if (zeroCamPanRoll)
            {
                return new Vector3 (cosPitch * (BODY.x - camPosition.x) - sinPitch * (BODY.z - camPosition.z),
                                    BODY.y - camPosition.y,
                                    sinPitch * (BODY.x - camPosition.x) + cosPitch * (BODY.z - camPosition.z));
            }
            else
            {
                if (R_BODY_CAM == null)
                {
                    R_BODY_CAM = new Matrix3(cosPitch * cosYaw,
                                             cosPitch * sinYaw,
                                            -sinPitch,
                                             sinRoll * sinPitch * cosYaw - cosRoll * sinYaw,
                                             sinRoll * sinPitch * sinYaw + cosRoll * cosYaw,
                                             sinRoll * cosPitch,
                                             cosRoll * sinPitch * cosYaw + sinRoll * sinYaw,
                                             cosRoll * sinPitch * sinYaw - sinRoll * cosYaw,
                                             cosRoll * cosPitch);
                }
                if (zeroCamDistance)
                    return (R_BODY_CAM * BODY);
                else
                    return (R_BODY_CAM * (BODY - camPosition));
            }
        }

        // Camera to Body system transformation
        public Vector3 CAMtoBODY(Vector3 CAM)
        {
            if (zeroCamPanRoll && zeroCamDistance)
            {
                return new Vector3 (cosPitch * CAM.x + sinPitch * CAM.z,
                                    CAM.y,
                                    -sinPitch * CAM.x + cosPitch * CAM.z);
            }
            else if (zeroCamPanRoll)
            {
                return new Vector3 (cosPitch * CAM.x + sinPitch * CAM.z + camPosition.x,
                                    CAM.y + camPosition.y,
                                    -sinPitch * CAM.x + cosPitch * CAM.z + camPosition.z);
            }
            else
            {
                if (R_CAM_BODY == null)
                {
                    R_CAM_BODY = new Matrix3(cosYaw * cosPitch,
                                            -sinYaw * cosRoll + cosYaw * sinPitch * sinRoll,
                                             sinYaw * sinRoll + cosYaw * sinPitch * cosRoll,
                                             sinYaw * cosPitch,
                                             cosYaw * cosRoll + sinYaw * sinPitch * sinRoll,
                                            -cosYaw * sinRoll + sinYaw * sinPitch * cosRoll,
                                            -sinPitch,
                                             cosPitch * sinRoll,
                                             cosPitch * cosRoll);
                }
                if (zeroCamDistance)
                    return (R_CAM_BODY * CAM);
                else
                    return (R_CAM_BODY * CAM + camPosition);
            }
        }

        // Camera frame coordinates of a pixel [m,n] (m:row, n:column)
        public Vector3 PIXELtoCAM(int m, int n)
        {
            return new Vector3 (-scaleFactor * m + offsetX,
                                scaleFactor * n - offsetY,
                                focalLength);
        }

        // Body frame coordinates of a pixel [m,n]
        public Vector3 PIXELtoBODY(int m, int n)
        {
            if (zeroCamPanRoll && zeroCamDistance)
            {
                return new Vector3 ((-scaleFactor * m + offsetX) * cosPitch + focalLength * sinPitch,
                                    scaleFactor * n - offsetY,
                                    (scaleFactor * m - offsetX) * sinPitch + focalLength * cosPitch);
            }
            else if (zeroCamPanRoll)
            {
                return new Vector3 ((-scaleFactor * m + offsetX) * cosPitch + focalLength * sinPitch + camPosition.x,
                                    scaleFactor * n - offsetY + camPosition.y,
                                    (scaleFactor * m - offsetX) * sinPitch + focalLength * cosPitch + camPosition.z);
            }
            else
                return CAMtoBODY(PIXELtoCAM(m, n));
        }
    }
}
