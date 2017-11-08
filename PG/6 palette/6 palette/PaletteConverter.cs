using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6_palette
{
    class FixedPaletteConverter
    {
        int[,] palette { get; set; }

        public FixedPaletteConverter()
        {
            fillPalette();
        }
        /*
         * fill 3-3-2 palette
         */
        private void fillPalette()
        {
            palette = new int[256, 3];
            for (int i = 0; i < 256; i++)
            {
                palette[i, 0] = (i >> 5) * 32;                palette[i, 1] = ((i >> 2) & 7) * 32;                palette[i, 2] = (i & 3) * 64;
            }
            
        }
        /*
         * parce 3-3-2 byte array to bitmap
         */
        private byte[] parceImageToByteArray(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            List<byte> imageData = new List<byte>();
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * i) + (4 * j)));
                    int tableIndex = ((PixelColor.R / 32) << 5) | ((PixelColor.G / 32) << 2) | (PixelColor.B/64);
                    imageData.AddRange(BitConverter.GetBytes(tableIndex));
                }
            }
            bmp.UnlockBits(data);
            return imageData.ToArray();
        }

        public byte[] GetImageData(Bitmap bmp)
        {
            List<byte> image = new List<byte>();
            image.AddRange(BitConverter.GetBytes(bmp.Width));
            image.AddRange(BitConverter.GetBytes(bmp.Height));
            image.AddRange(parceImageToByteArray(bmp));
            return image.ToArray();
        }
        /*
         * parce bitmap to 3-3-2 palette byte array
         */
        public Bitmap convertDataToBitmap(byte[] imgData)
        {

            Stream stream = new MemoryStream(imgData);
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            int Width = BitConverter.ToInt32(buffer, 0);
            stream.Read(buffer, 0, 4);
            int Height = BitConverter.ToInt32(buffer, 0);
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            byte[] buffer2 = new byte[4];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    stream.Read(buffer2, 0, 4);
                    int tableIndex = BitConverter.ToInt32(buffer2, 0);
                    int R = palette[tableIndex, 0];
                    int G = palette[tableIndex, 1];
                    int B = palette[tableIndex, 2];
                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                }
            }
            stream.Close();
            return bmp;
        }
    }

    
}
