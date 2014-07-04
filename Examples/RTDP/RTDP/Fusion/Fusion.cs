using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace RTDP
{
    public class Fusion
    {
        // Input infrared and visual bitmaps
        FPFImage irImage;
        ViImage viImage;

        // Mapping between infrared and visual pixels
        IrViMapper map;

        public Fusion(CAMProperties irCam, CAMProperties viCam)
        {
            this.map = new IrViMapper(irCam, viCam);
        }

        public void Init(FPFImage ir_img, ViImage vi_img, Metadata mt)
        {
            this.irImage = ir_img;
            this.viImage = vi_img;
            map.Init(mt);
        }

        public void MarkHotSpots(ArrayList hotSpotsList)
        {					
            BitmapData irData = irImage.BMP.LockBits(new Rectangle(0, 0, irImage.Width, irImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData viData = viImage.BMP.LockBits(map.ViLockRectangle, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* viptr = (byte*)(viData.Scan0);
                Point ir_pixel = new Point();
                float temp, kg;
                byte g;
                for (int y_vi = 0; y_vi < viData.Height; y_vi++)
                {
                    for (int x_vi = 0; x_vi < viData.Width; x_vi++)
                    {
                        if (map.LockVIpToIRp(new Point(x_vi, y_vi), ref ir_pixel))
                        {
                            temp = irImage.GetPixelTemp(ir_pixel.Y, ir_pixel.X);
                            if (temp > RTDPSettings.segmentationSettings.hotspotThreshold)
                            {
                                // Mark hot spot pixel
                                kg = 255 * (irImage.RangeTmax - temp) / (irImage.RangeTmax - RTDPSettings.segmentationSettings.hotspotThreshold);
                                g = (kg >= 255) ? (byte)255 : (kg <= 0) ? (byte)0 : (byte)kg;

                                *viptr     = (byte)((*viptr) >> 1);                   // blue
                                *(viptr+1) = (byte)(((*(viptr+1)) >> 1) + (g >> 1));  // green
                                *(viptr+2) = (byte)(((*(viptr+2)) >> 1) + 127);       // red
                            }
                            else
                            {
                                // Mark light blue background
                                (*viptr) = (byte)(127 + ((*viptr) >> 1)); // high BLUE   
                            }
                        }
                        viptr += 3;
                    }
                    viptr += viData.Stride - viData.Width * 3;
                }

                // Mark hot spot pixels
                //float k = 255 / (irImage.RangeTmax - 320);
                //Rectangle vi_rect = new Rectangle();
                foreach (HotSpot hotspot in hotSpotsList)
                {
                //    foreach (HSPixel ir_pxl in hotspot.Pixels)
                //    {
                //        if (map.IRpToLockVIr(new Point(ir_pxl.X, ir_pxl.Y), ref vi_rect))
                //        {
                //            // RED:    { 255,   0,   0 }
                //            // ORANGE: { 255, 127,   0 }  
                //            // YELLOW: { 255, 255,   0 } 

                //            //byte g = (ir_pxl.Grade == HSPixel.HSGrade.A) ? (byte)0 :
                //            //         (ir_pxl.Grade == HSPixel.HSGrade.B) ? (byte)136: 
                //            //                                               (byte)255;

                //            float kg = k * (irImage.RangeTmax - ir_pxl.T);
                //            byte g = (kg >= 255) ? (byte)255 : (kg <= 0) ? (byte)0 : (byte)kg;

                //            for (int y_vi = vi_rect.Top; y_vi < vi_rect.Bottom; y_vi++)
                //            {
                //                viptr = (byte*)(viData.Scan0) + (y_vi * viData.Stride) + (vi_rect.X * 3);
                //                for (int x_vi = vi_rect.Left; x_vi < vi_rect.Right; x_vi++)
                //                {
                //                    *viptr++ = (byte)((*viptr) >> 1); // blue
                //                    *viptr++ = (byte)(((*viptr) >> 1) + (g>>1));  // green
                //                    *viptr++ = (byte)(((*viptr) >> 1) + 127);  // red
                //                }
                //            }
                //        }
                //    }

                    // Draw black rectangle
                    int width = 10, space = 5;
                    Point ir_tl = new Point(hotspot.MinN, hotspot.MinM);
                    Point ir_br = new Point(hotspot.MaxN + 1, hotspot.MaxM + 1);
                    Point vi_tl = new Point();
                    Point vi_br = new Point();
                    if (map.IRpToLockVIp(ir_tl, ref vi_tl) && map.IRpToLockVIp(ir_br, ref vi_br))
                    {
                        vi_tl.X -= (width + space);
                        vi_tl.Y -= (width + space);
                        vi_br.X += (width + space - 1);
                        vi_br.Y += (width + space - 1);

                        if (vi_tl.X < 0) vi_tl.X = 0;
                        if (vi_tl.Y < 0) vi_tl.Y = 0;
                        if (vi_br.X > viImage.Width - 1) vi_br.X = viImage.Width - 1;
                        if (vi_br.Y > viImage.Height - 1) vi_br.Y = viImage.Height - 1;

                        for (int y_vi = vi_tl.Y; y_vi < vi_tl.Y + width; y_vi++)
                        {
                            viptr = (byte*)(viData.Scan0) + (y_vi * viData.Stride) + (vi_tl.X * 3);
                            for (int x_vi = vi_tl.X; x_vi <= vi_br.X; x_vi++)
                            {
                                *viptr++ = Color.Black.B;
                                *viptr++ = Color.Black.G;
                                *viptr++ = Color.Black.R;
                            }
                        }

                        for (int y_vi = vi_tl.Y + width; y_vi < vi_br.Y - width + 1; y_vi++)
                        {
                            viptr = (byte*)(viData.Scan0) + (y_vi * viData.Stride) + (vi_tl.X * 3);
                            for (int x_vi = vi_tl.X; x_vi < vi_tl.X + width; x_vi++)
                            {
                                *viptr++ = Color.Black.B;
                                *viptr++ = Color.Black.G;
                                *viptr++ = Color.Black.R;
                            }

                            viptr = (byte*)(viData.Scan0) + (y_vi * viData.Stride) + ((vi_br.X - width + 1) * 3);
                            for (int x_vi = vi_br.X - width + 1; x_vi <= vi_br.X; x_vi++)
                            {
                                *viptr++ = Color.Black.B;
                                *viptr++ = Color.Black.G;
                                *viptr++ = Color.Black.R;
                            }
                        }

                        for (int y_vi = vi_br.Y - width + 1; y_vi <= vi_br.Y; y_vi++)
                        {
                            viptr = (byte*)(viData.Scan0) + (y_vi * viData.Stride) + (vi_tl.X * 3);
                            for (int x_vi = vi_tl.X; x_vi <= vi_br.X; x_vi++)
                            {
                                *viptr++ = Color.Black.B;
                                *viptr++ = Color.Black.G;
                                *viptr++ = Color.Black.R;
                            }
                        }
                    }
                }
            }
 
            irImage.BMP.UnlockBits(irData);
            viImage.BMP.UnlockBits(viData);
        }

        public void Merge()
        {
            BitmapData irData = irImage.BMP.LockBits(new Rectangle(0, 0, irImage.Width, irImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData viData = viImage.BMP.LockBits(map.ViLockRectangle, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                Point ir_pixel = new Point();
                byte* viptr = (byte*)(viData.Scan0);
                for (int y_vi = 0; y_vi < viData.Height; y_vi++)
                {
                    for (int x_vi = 0; x_vi < viData.Width; x_vi++)
                    {
                        if (map.LockVIpToIRp(new Point(x_vi, y_vi), ref ir_pixel))
                        {
                            byte* irptr = (byte*)(irData.Scan0) + (ir_pixel.Y * irData.Stride) + (ir_pixel.X * 3);

                            *viptr++ = (byte)(((*viptr) >> 1) + ((*irptr++) >> 1));
                            *viptr++ = (byte)(((*viptr) >> 1) + ((*irptr++) >> 1));
                            *viptr++ = (byte)(((*viptr) >> 1) + ((*irptr++) >> 1));
                        }
                        else 
                        {
                            viptr += 3;
                        }
                    }
                    viptr += viData.Stride - viData.Width * 3;
                }
            }
            irImage.BMP.UnlockBits(irData);
            viImage.BMP.UnlockBits(viData);
        }

     }
}
