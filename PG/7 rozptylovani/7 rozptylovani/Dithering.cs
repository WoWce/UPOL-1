using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _7_rozptylovani
{
    class Dithering
    {
        public int ditheringMatrixSize;

        /*
         * 0 - grayscale
         * 1 - threshold dithering
         * 2 - random dithering
         */
        public unsafe Bitmap SimpleDithering(Bitmap original, int switchAlgorithm = 0)
        {
            Bitmap newBitmap;
            if (switchAlgorithm == 0)
                newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            else
                newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format1bppIndexed);

            BitmapData originalData = original.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData newData = newBitmap.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int pixelSize = 3;
            int pivot = 127;
            Random rand = new Random();
            for (int y = 0; y < original.Height; y++)
            {
                byte* originalByteData = (byte*)originalData.Scan0 + (y * originalData.Stride);

                byte* newByteData = (byte*)newData.Scan0 + (y * newData.Stride);

                for (int x = 0; x < original.Width; x++)
                {
                    byte grayColor;

                    if (switchAlgorithm == 0)
                    {
                        grayColor = (byte)((originalByteData[x * pixelSize] +
                                           originalByteData[x * pixelSize + 1] + 
                                           originalByteData[x * pixelSize + 2] )/3);

                        newByteData[x * pixelSize] = grayColor;
                        newByteData[x * pixelSize + 1] = grayColor;
                        newByteData[x * pixelSize + 2] = grayColor;
                    }
                    else if (switchAlgorithm == 1)
                    {
                        byte black = 0;
                        byte white = 255;
                        grayColor = originalByteData[x * pixelSize];
                        if (grayColor >= pivot)
                        {
                            newByteData[x * pixelSize] = white;
                            newByteData[x * pixelSize + 1] = white;
                            newByteData[x * pixelSize + 2] = white;
                        }
                        else
                        {
                            newByteData[x * pixelSize] = black;
                            newByteData[x * pixelSize + 1] = black;
                            newByteData[x * pixelSize + 2] = black;
                        }
                    }
                    else if (switchAlgorithm == 2)
                    {
                        pivot = rand.Next(0, 256);
                        byte black = 0;
                        byte white = 255;
                        grayColor = originalByteData[x * pixelSize];
                        if (grayColor >= pivot)
                        {
                            newByteData[x * pixelSize] = white;
                            newByteData[x * pixelSize + 1] = white;
                            newByteData[x * pixelSize + 2] = white;
                        }
                        else
                        {
                            newByteData[x * pixelSize] = black;
                            newByteData[x * pixelSize + 1] = black;
                            newByteData[x * pixelSize + 2] = black;
                        }
                    }
                }
            }

            newBitmap.UnlockBits(newData);
            original.UnlockBits(originalData);

            return newBitmap;

        }

        /// <summary>
        /// Map (matrix) dithering
        /// </summary>
        public unsafe Bitmap MatrixDithering(Bitmap gray)
        {
            Bitmap matrixBmp = new Bitmap(gray.Width * ditheringMatrixSize, gray.Height * ditheringMatrixSize, PixelFormat.Format1bppIndexed);
            int pixelSize = 3;
            Matrix ditheringMatrix = CreateDitheringMatrix(ditheringMatrixSize);

            BitmapData originalData = gray.LockBits(
               new Rectangle(0, 0, gray.Width, gray.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData matrixData = matrixBmp.LockBits(
              new Rectangle(0, 0, matrixBmp.Width, matrixBmp.Height),
              ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < gray.Height; y++)
            {
                byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

                for (int x = 0; x < gray.Width; x++)
                {
                    var color = oRow[x * pixelSize];
                    Matrix current = ResultDitheringMatrix(color, ditheringMatrix);

                    for (int j = 0; j < current.Size; j++)
                    {
                        byte* nRow = (byte*)matrixData.Scan0 + ((j + y * ditheringMatrixSize) * matrixData.Stride);
                        for (int i = 0; i < current.Size; i++)
                        {
                            nRow[(i + x * ditheringMatrixSize) * pixelSize] = (byte)current[i, j];
                            nRow[(i + x * ditheringMatrixSize) * pixelSize + 1] = (byte)current[i, j];
                            nRow[(i + x * ditheringMatrixSize) * pixelSize + 2] = (byte)current[i, j];
                        }
                    }
                }
            }

            gray.UnlockBits(originalData);
            matrixBmp.UnlockBits(matrixData);
            return matrixBmp;
        }
        /// <summary>
        /// Creates matrix to replace pixels
        /// </summary>
        private Matrix ResultDitheringMatrix(int inputValue, Matrix matrix)
        {
            Matrix result = new Matrix(matrix.Size);
            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    if (inputValue <= matrix[i, j])
                    {
                        result[i, j] = 0;
                    }
                    else result[i, j] = 255;
                }
            }
            return result;
        }

        private Matrix CreateDitheringMatrix(int n)
        {
            Matrix m = new Matrix(2);
            m[0, 0] = 0;
            m[0, 1] = 2;
            m[1, 0] = 3;
            m[1, 1] = 1;
            return CreateMatrix(m, n);
        }

        /// <summary>
        /// Creates dithering matrix(recursive)
        /// </summary>
        private Matrix CreateMatrix(Matrix m, int n)
        {
            if (m.GetSize() < n)
            {
                int mSize = m.GetSize();
                Matrix result = new Matrix(mSize * 2);
                Matrix mx4 = 4 * m;
                Matrix singleMatrix = new Matrix(mSize);
                singleMatrix.FillWithConst(1);
                for (int i = 0; i < mSize; i++)
                {
                    for (int j = 0; j < mSize; j++)
                    {
                        result[i, j] = mx4[i, j];
                    }
                }
                Matrix secondQuarter = mx4 + 2 * singleMatrix;
                for (int i = 0; i < mSize; i++)
                {
                    for (int j = mSize; j < mSize * 2; j++)
                    {
                        result[i, j] = secondQuarter[i, j - mSize];
                    }
                }
                Matrix thirdQuarter = mx4 + 3 * singleMatrix;
                for (int i = mSize; i < mSize * 2; i++)
                {
                    for (int j = 0; j < mSize; j++)
                    {
                        result[i, j] = thirdQuarter[i - mSize, j];
                    }
                }
                Matrix fourthQuarter = mx4 + singleMatrix;
                for (int i = mSize; i < mSize * 2; i++)
                {
                    for (int j = mSize; j < mSize * 2; j++)
                    {
                        result[i, j] = fourthQuarter[i - mSize, j - mSize];
                    }
                }
                return CreateMatrix(result, n);
            }
            else return m;

        }
    }
}