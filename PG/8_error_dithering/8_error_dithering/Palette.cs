using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_error_dithering
{
    class Palette
    {
        int[,] palette { get; set; }

        public Palette()
        {
            fillPalette();
        }
        
        private void fillPalette()
        {
            palette = new int[256, 3];
            for (int i = 0; i < 256; i++)
            {
                palette[i, 0] = (i >> 5) * 32;                palette[i, 1] = ((i >> 2) & 7) * 32;                palette[i, 2] = (i & 3) * 64;
            }

        }

        public unsafe Bitmap correctImage(Bitmap original, int switchAlgorithm = 0)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            Matrix redMatrix = new Matrix(original.Height, original.Width);
            Matrix greenMatrix = new Matrix(original.Height, original.Width);
            Matrix blueMatrix = new Matrix(original.Height, original.Width);
            blueMatrix.FillWithConst(0);
            greenMatrix.FillWithConst(0);
            redMatrix.FillWithConst(0);

            BitmapData originalData = original.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData newData = newBitmap.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int pixelSize = 3;
            int errorRed = 0;
            int errorGreen = 0;
            int errorBlue = 0;
            byte newColor;
            for (int y = 0; y < original.Height; y++)
            {
                byte* originalByteData = (byte*)originalData.Scan0 + (y * originalData.Stride);
                byte* newByteData = (byte*)newData.Scan0 + (y * newData.Stride);

                for (int x = 0; x < original.Width; x++)
                {
                    byte redColor;
                    byte greenColor;
                    byte blueColor;

                    redColor = originalByteData[x * pixelSize];
                    greenColor = originalByteData[x * pixelSize + 1];
                    blueColor = originalByteData[x * pixelSize + 2];

                    redColor += (byte)redMatrix[y, x];
                    greenColor += (byte)greenMatrix[y, x];
                    blueColor += (byte)blueMatrix[y, x];

                    int tableIndex = ((redColor / 32) << 5) 
                        | ((greenColor / 32) << 2) 
                        | (blueColor / 64);

                    errorRed = redColor - palette[tableIndex, 0];
                    errorGreen = greenColor - palette[tableIndex, 1];
                    errorBlue = blueColor - palette[tableIndex, 2];
                    if (switchAlgorithm == 0)
                        floydSteinberg(x, y, redMatrix, errorRed, greenMatrix, errorGreen, blueMatrix, errorBlue);  //Floyd-Steinberg
                    else if (switchAlgorithm == 1)
                        SierraAlgorithm(x, y, redMatrix, errorRed, greenMatrix, errorGreen, blueMatrix, errorBlue); //F. Sierra
                    else if (switchAlgorithm == 2)
                        jjnAlgorithm(x, y, redMatrix, errorRed, greenMatrix, errorGreen, blueMatrix, errorBlue);    //J. Jarvis, C. Judice, W. Ninke
                    else if (switchAlgorithm == 3)
                        stuckiAlgorithm(x, y, redMatrix, errorRed, greenMatrix, errorGreen, blueMatrix, errorBlue); //Stucki

                    newByteData[x * pixelSize] = redColor;
                    newByteData[x * pixelSize + 1] = greenColor;
                    newByteData[x * pixelSize + 2] = blueColor;
                }
            }
            original.UnlockBits(originalData);
            newBitmap.UnlockBits(newData);

            return newBitmap;
        }

        private void floydSteinberg(int x, int y, Matrix redMatrix, int errorRed, Matrix greenMatrix, int errorGreen, Matrix blueMatrix, int errorBlue)
        {
            if ((x + 1) < redMatrix.Width)
            {
                redMatrix[y, x + 1] += (int)((7 * errorRed) >> 4);
                greenMatrix[y, x + 1] += (int)((7 * errorGreen) >> 4);
                blueMatrix[y, x + 1] += (int)((7 * errorBlue) >> 4);
            }
            if ((y + 1) < redMatrix.Height)
            {
                if ((x - 1) > 0)
                {
                    redMatrix[y + 1, x - 1] += (int)((3 * errorRed) >> 4);
                    greenMatrix[y + 1, x - 1] += (int)((3 * errorGreen) >> 4);
                    blueMatrix[y + 1, x - 1] += (int)((3 * errorBlue) >> 4);
                }
                redMatrix[y + 1, x] += (int)((5 * errorRed) >> 4);
                greenMatrix[y + 1, x] += (int)((5 * errorGreen) >> 4);
                blueMatrix[y + 1, x] += (int)((5 * errorBlue) >> 4);
                if ((x + 1) < redMatrix.Height)
                {
                    redMatrix[y + 1, x + 1] += (int)((errorRed * 1) >> 4);
                    greenMatrix[y + 1, x + 1] += (int)((errorGreen * 1) >> 4);
                    blueMatrix[y + 1, x + 1] += (int)((errorBlue * 1) >> 4);
                }

            }
        }

        private void SierraAlgorithm(int x, int y, Matrix redMatrix, int errorRed, Matrix greenMatrix, int errorGreen, Matrix blueMatrix, int errorBlue)
        {
            if ((x + 1) < redMatrix.Width)
            {
                redMatrix[y, x + 1] += (int)(errorRed >> 1);
                greenMatrix[y, x + 1] += (int)(errorGreen >> 1);
                blueMatrix[y, x + 1] += (int)(errorBlue >> 1);
            }
            if ((y + 1) < redMatrix.Height)
            {
                if ((x - 1) > 0)
                {
                    redMatrix[y + 1, x - 1] += (int)(errorRed >> 2);
                    greenMatrix[y + 1, x - 1] += (int)(errorGreen >> 2);
                    blueMatrix[y + 1, x - 1] += (int)(errorBlue >> 2);
                }
                redMatrix[y + 1, x] += (int)(errorRed >> 4);
                greenMatrix[y + 1, x] += (int)(errorGreen >> 4);
                blueMatrix[y + 1, x] += (int)(errorBlue >> 4);
            }
        }

        private void jjnAlgorithm(int x, int y, Matrix redMatrix, int errorRed, Matrix greenMatrix, int errorGreen, Matrix blueMatrix, int errorBlue)
        {
            double divider = 48;
            if ((x + 1) < redMatrix.Width)
            {
                redMatrix[y, x + 1] += (int)(errorRed * 7 / divider);
                greenMatrix[y, x + 1] += (int)(errorGreen * 7 / divider);
                blueMatrix[y, x + 1] += (int)(errorBlue * 7 / divider);
            }
            if ((x + 2) < redMatrix.Width)
            {
                redMatrix[y, x + 2] += (int)(errorRed * 5 / divider);
                greenMatrix[y, x + 2] += (int)(errorGreen * 5 / divider);
                blueMatrix[y, x + 2] += (int)(errorBlue * 5 / divider);
            }
            if ((y + 1) < redMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    redMatrix[y + 1, x - 2] += (int)(errorRed >> 4);
                    greenMatrix[y + 1, x - 2] += (int)(errorGreen >> 4);
                    blueMatrix[y + 1, x - 2] += (int)(errorBlue >> 4);
                }
                if ((x - 1) > 0)
                {
                    redMatrix[y + 1, x - 1] += (int)(errorRed * 5 / divider);
                    greenMatrix[y + 1, x - 1] += (int)(errorGreen * 5 / divider);
                    blueMatrix[y + 1, x - 1] += (int)(errorBlue * 5 / divider);
                }

                redMatrix[y + 1, x] += (int)(errorRed * 7 / divider);
                greenMatrix[y + 1, x] += (int)(errorGreen * 7 / divider);
                blueMatrix[y + 1, x] += (int)(errorBlue * 7 / divider);

                if ((x + 1) < redMatrix.Width)
                {
                    redMatrix[y + 1, x + 1] += (int)(errorRed * 5 / divider);
                    greenMatrix[y + 1, x + 1] += (int)(errorGreen * 5 / divider);
                    blueMatrix[y + 1, x + 1] += (int)(errorBlue * 5 / divider);
                }
                if ((x + 2) < redMatrix.Width)
                {
                    redMatrix[y + 1, x + 2] += (int)(errorRed >> 4);
                    greenMatrix[y + 1, x + 2] += (int)(errorGreen >> 4);
                    blueMatrix[y + 1, x + 2] += (int)(errorBlue >> 4);
                }
            }
            if ((y + 2) < redMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    redMatrix[y + 2, x - 2] += (int)(errorRed / divider);
                    greenMatrix[y + 2, x - 2] += (int)(errorGreen / divider);
                    blueMatrix[y + 2, x - 2] += (int)(errorBlue / divider);
                }
                if ((x - 1) > 0)
                {
                    redMatrix[y + 2, x - 1] += (int)(errorRed >> 4);
                    greenMatrix[y + 2, x - 1] += (int)(errorGreen >> 4);
                    blueMatrix[y + 2, x - 1] += (int)(errorBlue >> 4);
                }

                redMatrix[y + 2, x] += (int)(errorRed * 5 / divider);
                greenMatrix[y + 2, x] += (int)(errorGreen * 5 / divider);
                blueMatrix[y + 2, x] += (int)(errorBlue * 5 / divider);

                if ((x + 1) < redMatrix.Width)
                {
                    redMatrix[y + 2, x + 1] += (int)(errorRed >> 4);
                    greenMatrix[y + 2, x + 1] += (int)(errorGreen >> 4);
                    blueMatrix[y + 2, x + 1] += (int)(errorBlue >> 4);
                }
                if ((x + 2) < redMatrix.Width)
                {
                    redMatrix[y + 2, x + 2] += (int)(errorRed / divider);
                    greenMatrix[y + 2, x + 2] += (int)(errorGreen / divider);
                    blueMatrix[y + 2, x + 2] += (int)(errorBlue / divider);
                }
            }
        }

        private void stuckiAlgorithm(int x, int y, Matrix redMatrix, int errorRed, Matrix greenMatrix, int errorGreen, Matrix blueMatrix, int errorBlue)
        {
            int divider = 42;
            if ((x + 1) < redMatrix.Width)
            {
                redMatrix[y, x + 1] += (int)(errorRed * 8 / divider);
                greenMatrix[y, x + 1] += (int)(errorGreen * 8 / divider);
                blueMatrix[y, x + 1] += (int)(errorBlue * 8 / divider);
            }
            if ((x + 2) < redMatrix.Width)
            {
                redMatrix[y, x + 2] += (int)(errorRed * 4 / divider);
                greenMatrix[y, x + 2] += (int)(errorGreen * 4 / divider);
                blueMatrix[y, x + 2] += (int)(errorBlue * 4 / divider);
            }
            if ((y + 1) < redMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    redMatrix[y + 1, x - 2] += (int)(errorRed / 21);
                    greenMatrix[y + 1, x - 2] += (int)(errorGreen / 21);
                    blueMatrix[y + 1, x - 2] += (int)(errorBlue / 21);
                }
                if ((x - 1) > 0)
                {
                    redMatrix[y + 1, x - 1] += (int)(errorRed * 4 / divider);
                    greenMatrix[y + 1, x - 1] += (int)(errorGreen * 4 / divider);
                    blueMatrix[y + 1, x - 1] += (int)(errorBlue * 4 / divider);
                }
                redMatrix[y + 1, x] += (int)(errorRed * 8 / divider);
                greenMatrix[y + 1, x] += (int)(errorGreen * 8 / divider);
                blueMatrix[y + 1, x] += (int)(errorBlue * 8 / divider);
                if ((x + 1) < redMatrix.Width)
                {
                    redMatrix[y + 1, x + 1] += (int)(errorRed * 4 / divider);
                    greenMatrix[y + 1, x + 1] += (int)(errorGreen * 4 / divider);
                    blueMatrix[y + 1, x + 1] += (int)(errorBlue * 4 / divider);
                }
                if ((x + 2) < redMatrix.Width)
                {
                    redMatrix[y + 1, x + 2] += (int)(errorRed / 21);
                    greenMatrix[y + 1, x + 2] += (int)(errorGreen / 21);
                    blueMatrix[y + 1, x + 2] += (int)(errorBlue / 21);
                }
            }
            if ((y + 2) < redMatrix.Height)
            {
                if ((x - 2) > 0)
                {
                    redMatrix[y + 2, x - 2] += (int)(errorRed / divider);
                    greenMatrix[y + 2, x - 2] += (int)(errorGreen / divider);
                    blueMatrix[y + 2, x - 2] += (int)(errorBlue / divider);
                }
                if ((x - 1) > 0)
                {
                    redMatrix[y + 2, x - 1] += (int)(errorRed / 21);
                    greenMatrix[y + 2, x - 1] += (int)(errorGreen / 21);
                    blueMatrix[y + 2, x - 1] += (int)(errorBlue / 21);
                }

                redMatrix[y + 2, x] += (int)(errorRed * 4 / divider);
                greenMatrix[y + 2, x] += (int)(errorGreen * 4 / divider);
                blueMatrix[y + 2, x] += (int)(errorBlue * 4 / divider);

                if ((x + 1) < redMatrix.Width)
                {
                    redMatrix[y + 2, x + 1] += (int)(errorRed / 21);
                    greenMatrix[y + 2, x + 1] += (int)(errorGreen / 21);
                    blueMatrix[y + 2, x + 1] += (int)(errorBlue / 21);
                }
                if ((x + 2) < redMatrix.Width)
                {
                    redMatrix[y + 2, x + 2] += (int)(errorRed / divider);
                    greenMatrix[y + 2, x + 2] += (int)(errorGreen / divider);
                    blueMatrix[y + 2, x + 2] += (int)(errorBlue / divider);
                }
            }
        }

    }
}
