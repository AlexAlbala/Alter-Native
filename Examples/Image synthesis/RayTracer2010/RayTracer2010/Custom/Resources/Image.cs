using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using Custom.Resources;
using Custom.Math;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Custom.Resources
{
    public class ImageRef : PooledResourceReference
    {
        public ImageRef(PooledResourceEntry r)
            : base(r)
        {
        }

        public Image img
        {
            get
            {
                return (Image)reference.resource;
            }
        }
    }

    public class ImageLoader : IResourceLoader
    {
        public IDisposable loadResource(string name)
        {
            return new Image(name);
        }

        public PooledResourceReference wrapReference(PooledResourceEntry res)
        {
            return new ImageRef(res);
        }
    }

    public class Image : IDisposable
    {
        public static int images_loaded = 0;
        public string name;
        public int tex_id = 0;
        public int w = 0, h = 0;

        public Image(string name)
        {
            this.name = name;
            Bitmap bmp = new Bitmap(name);

            images_loaded++;

            w = bmp.Size.Width;
            h = bmp.Size.Height;

            // flip the image and apply transparency
            byte[] pixels = new byte[w * h * 4];
            unsafe
            {
                Rectangle rect = new Rectangle(0, 0, w, h);
                BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                byte* src = (byte*)data.Scan0;

                for (int y = 0; y < h; y++)
                {
                    int yf = h - y - 1;

                    for (int x = 0; x < w; x++)
                    {
                        byte b = src[(yf * w + x) * 4 + 0];
                        byte g = src[(yf * w + x) * 4 + 1];
                        byte r = src[(yf * w + x) * 4 + 2];
                        byte a = src[(yf * w + x) * 4 + 3];
                        if (r == 255 && g == 0 && b == 255 && a == 255)
                        {
                            pixels[(y * w + x) * 4 + 0] = 0;
                            pixels[(y * w + x) * 4 + 1] = 0;
                            pixels[(y * w + x) * 4 + 2] = 0;
                            pixels[(y * w + x) * 4 + 3] = 0;
                        }
                        else
                        {
                            pixels[(y * w + x) * 4 + 0] = r;
                            pixels[(y * w + x) * 4 + 1] = g;
                            pixels[(y * w + x) * 4 + 2] = b;
                            pixels[(y * w + x) * 4 + 3] = a;
                        }
                    }
                }

                bmp.UnlockBits(data);
            }

            GL.GenTextures(1, out tex_id);
            GL.BindTexture(TextureTarget.Texture2D, tex_id);

            //Glu.gluBuild2DMipmaps(EnableCap.Texture2D, 4, w, h, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

        }

        public void Dispose()
        {
            GL.DeleteTextures(1, ref tex_id);
            images_loaded--;
        }

        public void bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, tex_id);
        }

        public void draw(ref vect2d dst)
        {
            // find texture coordinates
            float texX1 = 0;
            float texY1 = 0;
            float texX2 = 1;
            float texY2 = 1;

            // bind it
            GL.BindTexture(TextureTarget.Texture2D, tex_id);

            // draw it
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(texX1, texY1);
            GL.Vertex2((float)dst.x, (float)dst.y);

            GL.TexCoord2(texX2, texY1);
            GL.Vertex2((float)(dst.x + w), (float)dst.y);

            GL.TexCoord2(texX2, texY2);
            GL.Vertex2((float)(dst.x + w), (float)(dst.y + h));

            GL.TexCoord2(texX1, texY2);
            GL.Vertex2((float)dst.x, (float)(dst.y + h));
            GL.End();
        }

        public void draw(ref vect2d dst, ref vect2d src, ref vect2d dim)
        {
            // find texture coordinates
            float texX1 = (float)(src.x / w);
            float texY1 = (float)(src.y / h);
            float texX2 = (float)((src.x + dim.x) / w);
            float texY2 = (float)((src.y + dim.y) / h);

            // bind it
            GL.BindTexture(TextureTarget.Texture2D, tex_id);

            // draw it
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(texX1, texY1);
            GL.Vertex2((float)dst.x, (float)dst.y);

            GL.TexCoord2(texX2, texY1);
            GL.Vertex2((float)(dst.x + dim.x), (float)dst.y);

            GL.TexCoord2(texX2, texY2);
            GL.Vertex2((float)(dst.x + dim.x), (float)(dst.y + dim.y));

            GL.TexCoord2(texX1, texY2);
            GL.Vertex2((float)dst.x, (float)(dst.y + dim.y));
            GL.End();
        }
    }
}
