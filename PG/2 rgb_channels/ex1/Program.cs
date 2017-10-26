using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ex1
{
    
    class Program : GameWindow
    {
        public Program(string path, double screenSize)
            : base(900, 720, GraphicsMode.Default, "EX1")
        {
            this.path = path;
            this.screenSize = screenSize;
        }

        private double screenSize; 
        private Bitmap map;
        private int texture;
        private int textureR;
        private int textureG;
        private int textureB;
        private string path;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            map = LoadBitmap(@path);
            texture = bitmapToTex(map);
            getImageInfo(map);
            Bitmap bitmap2 = LoadBitmap(@path, "R");
            textureR = bitmapToTex(bitmap2);
            Bitmap bitmap3 = LoadBitmap(@path, "G");
            textureG = bitmapToTex(bitmap3);
            Bitmap bitmap4 = LoadBitmap(@path, "B");
            textureB = bitmapToTex(bitmap4);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.Enable(EnableCap.Texture2D);
        }

        //create texture from a bitmap
        private int bitmapToTex(Bitmap bitmap)
        {
            int tex = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return tex;
        }

        public Bitmap LoadBitmap(string path, string channel = "rgb")
        {
            int tex = GL.GenTexture();
            Bitmap bitmap = new Bitmap(path);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Prepare channel bitmap
            var channelBitmap = new Bitmap(data.Width, data.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            switch (channel)
            {
                case "R":
                    for (int y = 0; y < data.Height; y++)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * y) + (4 * x)));
                            if (PixelColor.R > 0 & PixelColor.R <= 255)
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(PixelColor.R, 0, 0));
                            }
                            else
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }
                    return channelBitmap;
                case "G":
                    for (int y = 0; y < data.Height; y++)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * y) + (4 * x)));
                            if (PixelColor.G > 0 & PixelColor.G <= 255)
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(0, PixelColor.G, 0));
                            }
                            else
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }
                    return channelBitmap;
                case "B":
                    for (int y = 0; y < data.Height; y++)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * y) + (4 * x)));
                            if (PixelColor.B > 0 & PixelColor.B <= 255)
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(0, 0, PixelColor.B));
                            }
                            else
                            {
                                channelBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }
                    return channelBitmap;
                default:
                    bitmap.UnlockBits(data);
                    return bitmap;
            }
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            //positioning of textures
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(0, -1);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textureG);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, 0);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textureR);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 0);
            GL.Vertex2(0, 1);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex2(0, 0);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textureB);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);
            GL.End();

            SwapBuffers();
        }
        //count PPI of screen
        public double countPPI(double phSize)
        {
            int height = SystemInformation.PrimaryMonitorSize.Height;
            int width = SystemInformation.PrimaryMonitorSize.Width;
            return Math.Sqrt(height*height+width*width)/phSize;
        }
        //ukol 1/2, Image Info
        public void getImageInfo(Bitmap bitmap)
        {
            const int dpi150 = 150;
            const int dpi300 = 300;
            const int dpi600 = 600;
            int width = bitmap.Width;
            int height = bitmap.Height;
            Console.WriteLine("Image resolution " + bitmap.Size);
            double ppi = countPPI(17.3);
            Console.WriteLine("On this screen(" + ppi + "ppi) image width = " + width / ppi + " inch, height = " + height / ppi + " inch");
            Console.WriteLine("Size of printed image:");
            Console.WriteLine("150 DPI: width = " + toCm(width / dpi150) + " cm, height = " + toCm(height / dpi150) + " cm.");
            Console.WriteLine("300 DPI: width = " + toCm(width / dpi300) + " cm, height = " + toCm(height / dpi300) + " cm.");
            Console.WriteLine("600 DPI: width = " + toCm(width / dpi600) + " cm, height = " + toCm(height / dpi600) + " cm.");
            Console.WriteLine("Size = " + height * width / 1000000 + "MPix");
        }

        private double toCm(double cm)
        {
            return cm * 2.54;
        }

        static void Main(string[] args)
        {
            string path = "e:/2.Study/UPOL/semestr 5/PG/MARBLES.bmp"; //path for *.bmp image
            double screenSize = 17.3; //screen size in inches
            using (Program game = new Program(path, screenSize))
            {
                game.Run(30.0);
            }
        }
    }
}
