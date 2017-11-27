using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _9_noise_reduction
{
    class ImageBlur
    {
        /// <summary>
        /// Blur with median algorithm
        /// </summary>
        public Bitmap ApplyMedianFilter(Bitmap sourceBitmap, int matrixSize)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            List<int> pixels = new List<int>();
            byte[] originalByteData = new byte[sourceData.Stride * sourceData.Height];
            byte[] newByteData = new byte[sourceData.Stride * sourceData.Height];
            byte[] middleBuffer;
            int filterOffset = (matrixSize - 1) / 2;
            int imageOffset = 0;
            int offset = 0;
            int median = matrixSize * matrixSize / 2;

            Marshal.Copy(sourceData.Scan0, originalByteData, 0, originalByteData.Length);
            sourceBitmap.UnlockBits(sourceData);
            
            for (int y = filterOffset; y <
                sourceBitmap.Height - filterOffset; y++)
            {
                for (int x = filterOffset; x <
                    sourceBitmap.Width - filterOffset; x++)
                {
                    offset = y * sourceData.Stride + x * 4;
                    pixels = new List<int>();

                    for (int i = -filterOffset; i <= filterOffset; i++)
                    {
                        for (int j = -filterOffset; j <= filterOffset; j++)
                        {
                            imageOffset = offset + (j * 4) + (i * sourceData.Stride);
                            pixels.Add(BitConverter.ToInt32(originalByteData, imageOffset));
                        }
                    }
                    pixels.Sort();
                    
                    middleBuffer = BitConverter.GetBytes(pixels[median]);

                    newByteData[offset] = middleBuffer[0];
                    newByteData[offset + 1] = middleBuffer[1];
                    newByteData[offset + 2] = middleBuffer[2];
                    newByteData[offset + 3] = middleBuffer[3];
                }
            }

            Bitmap outputBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height),
                       ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(newByteData, 0, resultData.Scan0, newByteData.Length);
            outputBitmap.UnlockBits(resultData);

            return outputBitmap;
        }
    }
}
