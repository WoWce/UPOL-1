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
    class ImageEditor
    {
        /// <summary>
        /// Fast grayscale using pointers
        /// </summary>
        public unsafe Bitmap ToGray(Bitmap original)
        {
            int pixelSize = 3;

            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);

            BitmapData originalData = original.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData newData = newBitmap.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int y = 0; y < original.Height; y++)
            {
                byte* originalByteData = (byte*)originalData.Scan0 + (y * originalData.Stride);
                byte* newByteData = (byte*)newData.Scan0 + (y * newData.Stride);

                for (int x = 0; x < original.Width; x++)
                {
                    byte grayColor;
                    grayColor = (byte)((originalByteData[x * pixelSize] +
                                        originalByteData[x * pixelSize + 1] +
                                        originalByteData[x * pixelSize + 2]) / 3);
                    
                    newByteData[x * pixelSize] = grayColor;
                    newByteData[x * pixelSize + 1] = grayColor;
                    newByteData[x * pixelSize + 2] = grayColor;
                }
            }
            original.UnlockBits(originalData);
            newBitmap.UnlockBits(newData);

            return newBitmap;
        }
        
    }

    
}
