using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_error_dithering
{
    class Dithering
    {
        public unsafe Bitmap SimpleDithering(Bitmap original, int switchAlgorithm = 0)
        {
            int pixelSize = 3;
            int pivot = 128;
            int inOutDifference = 0;
            byte black = 0;
            byte white = 255;
            byte newColor;

            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            Matrix bitmapMatrix = new Matrix(original.Height, original.Width);
            bitmapMatrix.FillWithConst(0);

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

                    if (switchAlgorithm == 0)
                    {
                        grayColor = (byte)(originalByteData[x * pixelSize] * 0.299 + 
                                           originalByteData[x * pixelSize + 1] * 0.587 +
                                           originalByteData[x * pixelSize + 2] * 0.144);
                        newColor = grayColor;
                    } else
                    {
                        
                        grayColor = originalByteData[x * pixelSize];
                        grayColor += (byte)bitmapMatrix[y, x];
                        if (grayColor >= pivot)
                            newColor = white;
                        else
                            newColor = black;

                        inOutDifference = grayColor - newColor;
                        //choose algorithm
                        if (switchAlgorithm == 1)
                            floydSteinberg(x, y, bitmapMatrix, inOutDifference);    //Floyd-Steinberg
                        else if (switchAlgorithm == 2)
                            SierraAlgorithm(x, y, bitmapMatrix, inOutDifference);   //F. Sierra
                        else if (switchAlgorithm == 3)
                            jjnAlgorithm(x, y, bitmapMatrix, inOutDifference);      //J. Jarvis, C. Judice, W. Ninke
                        else if (switchAlgorithm == 4)
                            stuckiAlgorithm(x, y, bitmapMatrix, inOutDifference);   //Stucki
                    }
                    newByteData[x * pixelSize] = newColor;
                    newByteData[x * pixelSize + 1] = newColor;
                    newByteData[x * pixelSize + 2] = newColor;
                }
            }
            original.UnlockBits(originalData);
            newBitmap.UnlockBits(newData);

            return newBitmap;
        }

        private void floydSteinberg(int x, int y, Matrix bitmapMatrix, int error)
        {
            if ((x + 1) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 1] += (int)((7 * error) >> 4);
            }
            if ((y + 1) < bitmapMatrix.Height)
            {
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 1, x - 1] += (int)((3 * error) >> 4);
                }
                bitmapMatrix[y + 1, x] += (int)((5 * error) >> 4);
                if ((x + 1) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 1, x + 1] += (int)((error * 1) >> 4);
                }

            }
        }

        private void SierraAlgorithm(int x, int y, Matrix bitmapMatrix, int error)
        {
            if ((x + 1) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 1] += (int)(error >> 1);
            }
            if ((y + 1) < bitmapMatrix.Height)
            {
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 1, x - 1] += (int)(error >> 2);
                }
                bitmapMatrix[y + 1, x] += (int)(error >> 4);
            }
        }

        private void jjnAlgorithm(int x, int y, Matrix bitmapMatrix, int error)
        {
            double   divider = 48;
            if ((x + 1) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 1] += (int)(error * 7 / divider);
            }
            if ((x + 2) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 2] += (int)(error * 5 / divider);
            }
            if ((y + 1) < bitmapMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    bitmapMatrix[y + 1, x - 2] += (int)(error >> 4);
                }
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 1, x - 1] += (int)(error * 5 / divider);
                }
                bitmapMatrix[y + 1, x] += (int)(error * 7 / divider);
                if ((x + 1) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 1, x + 1] += (int)(error * 5 / divider);
                }
                if ((x + 2) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 1, x + 2] += (int)(error >> 4);
                }
            }
            if ((y + 2) < bitmapMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    bitmapMatrix[y + 2, x - 2] += (int)(error / divider);
                }
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 2, x - 1] += (int)(error >> 4);
                }
                bitmapMatrix[y + 2, x] += (int)(error * 5 / divider);
                if ((x + 1) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 2, x + 1] += (int)(error >> 4);
                }
                if ((x + 2) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 2, x + 2] += (int)(error / divider);
                }
            }
        }

        private void stuckiAlgorithm(int x, int y, Matrix bitmapMatrix, int error)
        {
            double divider = 42;
            if ((x + 1) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 1] += (int)(error * 8 / divider);
            }
            if ((x + 2) < bitmapMatrix.Width)
            {
                bitmapMatrix[y, x + 2] += (int)(error * 4 / divider);
            }
            if ((y + 1) < bitmapMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    bitmapMatrix[y + 1, x - 2] += (int)(error / 21);
                }
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 1, x - 1] += (int)(error * 4 / divider);
                }
                bitmapMatrix[y + 1, x] += (int)(error * 8 / divider);
                if ((x + 1) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 1, x + 1] += (int)(error * 4 / divider);
                }
                if ((x + 2) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 1, x + 2] += (int)(error / 21);
                }
            }
            if ((y + 2) < bitmapMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    bitmapMatrix[y + 2, x - 2] += (int)(error / divider);
                }
                if ((x - 1) > 0)
                {
                    bitmapMatrix[y + 2, x - 1] += (int)(error / 21);
                }
                bitmapMatrix[y + 2, x] += (int)(error * 4 / divider);
                if ((x + 1) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 2, x + 1] += (int)(error / 21);
                }
                if ((x + 2) < bitmapMatrix.Width)
                {
                    bitmapMatrix[y + 2, x + 2] += (int)(error / divider);
                }
            }
        }

    }
}
