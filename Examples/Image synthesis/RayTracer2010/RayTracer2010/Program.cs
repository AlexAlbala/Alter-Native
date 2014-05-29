using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Custom.Math;
using Custom;

namespace RayTracer2010
{
    class Program
    {
        static void Main(string[] args)
        {
            //--------------------------------------------------------------------------------
            // Build the scene
            //--------------------------------------------------------------------------------
            double t_init = Time.getTime();
            Camera camera = new Camera()
            {
                width = 600,
                height = 400,
                subsamples = 1,
                fov_y = 40,
                pos = new vect3d(0, .75, 3)
            };

            var occluder = new GroupList();

            var plane = new Plane(new vect3d(0, 0, 0), new vect3d(1, 0, 0), new vect3d(1, 0, -1), new ShadowShader());
            //var plane = new Plane(new vect3d(0, 0, 0), new vect3d(1, 0, 0), new vect3d(1, 0, -1), new TextureShader("../../../../resources/images/wall_512_3_05_sml2.jpg"));
            plane.x *= .3;
            plane.y *= .3;
            occluder.add(plane);

            //occluder.add(new Sphere(new vect3d(0,.5,0), .5, new ShadowShader()));
            //occluder.add(new Sphere(new vect3d(0, .5, 0), .5, new TextureSphereShader("../../../../resources/images/earthmap1k.jpg")));

            GroupList obj = load_obj("../../../../resources/models/armadilloman2.obj", new ShadowShader());
            occluder.add(new GroupTree(obj.list, obj.shader));

            var scene = new Scene()
            {
                occluder = occluder
            };
            scene.lights.Add(new Light() { color = new vect3d(1, 1, 1) * 60, pos = new vect3d(-4, 8, 8) });
            scene.lights.Add(new Light() { color = new vect3d(1, 1, 1) * 40, pos = new vect3d(1, 15, 10) });

            //--------------------------------------------------------------------------------
            // Render the scene
            //--------------------------------------------------------------------------------
            double t_start = Time.getTime();
            System.Console.WriteLine("Time taken to create scene = {0}s", (t_start - t_init) / 1000);

            Bitmap bmp = camera.create_image(scene);

            double t_end = Time.getTime();
            System.Console.WriteLine("Time taken to render = {0}s", (t_end - t_start) / 1000);

            //--------------------------------------------------------------------------------
            // Write the image to disk
            //--------------------------------------------------------------------------------
            bmp.Save("output.png");
        }

        static GroupList load_obj(string fn, Shader shader)
        {
            var mesh = new GroupList();

            List<vect3d> verts = new List<vect3d>();
            List<vect3d> norms = new List<vect3d>();
            List<vect3d> texc = new List<vect3d>();

            using (TextReader reader = File.OpenText(fn))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var toks = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (toks.Count() == 0)
                        continue;

                    if (toks[0] == "v")
                    {
                        verts.Add(new vect3d(double.Parse(toks[1]), double.Parse(toks[2]), double.Parse(toks[3])));
                    }
                    else if (toks[0] == "vt")
                    {
                        texc.Add(new vect3d(double.Parse(toks[1]), double.Parse(toks[2]), 0));
                    }
                    else if (toks[0] == "vn")
                    {
                        norms.Add(new vect3d(double.Parse(toks[1]), double.Parse(toks[2]), double.Parse(toks[3])));
                    }
                    else if (toks[0] == "f")
                    {
                        var vals0 = toks[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        var vals1 = toks[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        var vals2 = toks[3].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        int i0 = int.Parse(vals0[0]) - 1;
                        int i1 = int.Parse(vals1[0]) - 1;
                        int i2 = int.Parse(vals2[0]) - 1;

                        var t = new Triangle(verts[i0], verts[i1], verts[i2], shader);

                        if (texc.Count() != 0)
                        {
                            int j0 = int.Parse(vals0[1]) - 1;
                            int j1 = int.Parse(vals1[1]) - 1;
                            int j2 = int.Parse(vals2[1]) - 1;

                            t.t0 = texc[j0];
                            t.t1 = texc[j1];
                            t.t2 = texc[j2];
                        }
                        if (norms.Count() != 0)
                        {
                            int k0 = int.Parse(vals0[2]) - 1;
                            int k1 = int.Parse(vals1[2]) - 1;
                            int k2 = int.Parse(vals2[2]) - 1;

                            t.n0 = norms[k0];
                            t.n1 = norms[k1];
                            t.n2 = norms[k2];
                        }

                        mesh.add(t);
                    }
                }
            }

            return mesh;
        }
    }
}
