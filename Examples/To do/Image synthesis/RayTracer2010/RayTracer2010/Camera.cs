using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using Custom.Math;
using Custom;

namespace RayTracer2010
{
    public class Camera
    {
        public vect3d pos = new vect3d(0, 0, 0);
        public quaternion orient = quaternion.makeIdentity();
        public int width = 600, height = 400;
        public double fov_y = 70;
        public int subsamples = 2;

        public Bitmap create_image(Scene scene)
        {
            Bitmap bmp = new Bitmap(width, height);

            // get the orientation of the camera
            matrix4d rotmat = orient.getMatrix();
            vect3d rt = new vect3d(rotmat.m[0], rotmat.m[1], rotmat.m[2]);
            vect3d up = new vect3d(rotmat.m[4], rotmat.m[5], rotmat.m[6]);
            vect3d fwd = new vect3d(rotmat.m[8], rotmat.m[9], rotmat.m[10]);

            // convert the orientation to a 3D screen
            double aspect = (double)width / (double)height;
            double h = Math.Tan(fov_y * Math.PI / 360.0);
            up *= -h * 2;
            rt *= aspect * h * 2;
            fwd *= -1;

            // 2D screen conversions
            vect3d dx = rt / width;
            vect3d dy = up / height;
            vect3d corner = fwd - rt * .5 - up * .5;

            // expose each pixel
            Ray r = new Ray();
            r.scene = scene;

            double subsample_res = 1.0 / subsamples;

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    vect3d color = new vect3d(0, 0, 0);

                    for (int jj = 0; jj < subsamples; jj++)
                    {
                        for (int ii = 0; ii < subsamples; ii++)
                        {
                            // set ray properties
                            r.pos = this.pos;
                            r.hit_dist = Double.MaxValue;
                            r.color = new vect3d(0, 0, 0);
                            r.hit_object = null;
                            
                            r.dir = corner + dx * (i + (ii+.5/*Rand.rand.NextDouble()*/) * subsample_res) + dy * (j + (jj+.5/*Rand.rand.NextDouble()*/) * subsample_res);
                            r.dir.normalize();

                            // trace the ray
                            scene.trace(r);
                            if (r.hit_object != null)
                            {
                                r.hit_object.shader.colorize(r);
                                color += r.color;
                            }
                        }
                    }

                    color *= subsample_res * subsample_res;

                    // gamma correct the color
                    double brightness = color.length();
                    color /= Math.Sqrt(brightness);

                    // store the color
                    int red = clamp(color.x);
                    int green = clamp(color.y);
                    int blue = clamp(color.z);

                    bmp.SetPixel(i, j, Color.FromArgb(255, red, green, blue));
                }
            }

            return bmp;
        }

        int clamp(double v)
        {
            int i = (int)(v * 256);
            if (i < 0)
                i = 0;
            else if (i > 255)
                i = 255;
            return i;
        }
    }
}
