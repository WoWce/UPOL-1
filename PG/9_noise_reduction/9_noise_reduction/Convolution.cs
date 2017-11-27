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
    class Convolution
    {
        /// <summary>
        /// Applies filter using 2 matrices
        /// </summary>
        /// <typeparam name="T">Filter subclass</typeparam>
        /// <typeparam name="S">Filter subclass</typeparam>
        /// <param name="sourceBitmap">Bitmap to be processed</param>
        /// <param name="XFilter">First matrix</param>
        /// <param name="YFilter">Second matrix</param>
        /// <param name="coefficient">Coefficient c</param>
        /// <returns>Bitmap</returns>
        public Bitmap ApplyMask<T, S>(Bitmap sourceBitmap, T XFilter, S YFilter, double coefficient)
            where T:Filter where S:Filter
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                             ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] originalByteData = new byte[sourceData.Stride * sourceData.Height];
            byte[] newByteData = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, originalByteData, 0, originalByteData.Length);
            Bitmap outputBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            
            BitmapData resultData = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height), 
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            sourceBitmap.UnlockBits(sourceData);
            
            double blueX = 0.0;
            double greenX = 0.0;
            double redX = 0.0;
            double blueY = 0.0;
            double greenY = 0.0;
            double redY = 0.0;
            double resultBlue = 0.0;
            double resultGreen = 0.0;
            double resultRed = 0.0;
            
            int maskOffset = (XFilter.FilterMatrix.GetLength(0) - 1) / 2;
            int imageOffset = 0;
            int byteOffset = 0;


            for (int y = maskOffset; y < sourceBitmap.Height - maskOffset; y++)
            {
                for (int x = maskOffset; x < sourceBitmap.Width - maskOffset; x++)
                {
                    blueX = greenX = redX = 0;
                    blueY = greenY = redY = 0;
                    resultBlue = resultGreen = resultRed = 0.0;
                    byteOffset = y * sourceData.Stride + x * 4;

                    for (int i = -maskOffset; i <= maskOffset; i++)
                    {
                        for (int j = -maskOffset; j <= maskOffset; j++)
                        {
                            imageOffset = byteOffset + (j * 4) + (i * sourceData.Stride);
                            
                            blueX += (double)(originalByteData[imageOffset]) 
                                * XFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                            greenX += (double)(originalByteData[imageOffset + 1]) 
                                * XFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                            redX += (double)(originalByteData[imageOffset + 2]) 
                                * XFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                            
                            blueY += (double)(originalByteData[imageOffset]) 
                                * YFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                            greenY += (double)(originalByteData[imageOffset + 1]) 
                                * YFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                            redY += (double)(originalByteData[imageOffset + 2]) 
                                * YFilter.FilterMatrix[i + maskOffset, j + maskOffset];
                        }
                    }
                    
                    resultBlue = maskAddition(blueX, blueY);
                    resultGreen = maskAddition(greenX, greenY);
                    resultRed = maskAddition(redX, redY);
                    
                    newByteData[byteOffset] = (byte)(originalByteData[byteOffset] + resultBlue * coefficient);
                    newByteData[byteOffset + 1] = (byte)(originalByteData[byteOffset + 1] + resultGreen * coefficient);
                    newByteData[byteOffset + 2] = (byte)(originalByteData[byteOffset + 2] + resultRed * coefficient);
                    newByteData[byteOffset + 3] = 255;
                }
            }
            
            Marshal.Copy(newByteData, 0, resultData.Scan0, newByteData.Length);
            outputBitmap.UnlockBits(resultData);
            
            return outputBitmap;
        }

        /// <summary>
        /// Applies filter using matrix
        /// </summary>
        /// <typeparam name="T">Filter subclass</typeparam>
        /// <param name="sourceBitmap">Bitmap to be processed</param>
        /// <param name="filterMatrix">Filter matrix</param>
        /// <param name="coefficient">Coefficient c</param>
        /// <returns>Bitmap</returns>
        public Bitmap ApplyMask<T>(Bitmap sourceBitmap, T filterMatrix, double coefficient, double oroginalCoeff = 1)
            where T : Filter
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                             ImageLockMode.ReadOnly,
                                        PixelFormat.Format32bppArgb);

            double factor = filterMatrix.Factor;
            byte[] originalByteData = new byte[sourceData.Stride * sourceData.Height];
            byte[] newByteData = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, originalByteData, 0, originalByteData.Length);
            Bitmap outputBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            
            BitmapData resultData = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height), 
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            
            sourceBitmap.UnlockBits(sourceData);

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;
            int maskOffset = (filterMatrix.FilterMatrix.GetLength(0) - 1) / 2;
            int imageOffset = 0;
            int offset = 0;

            for (int y = maskOffset; y < sourceBitmap.Height - maskOffset; y++)
            {
                for (int x = maskOffset; x < sourceBitmap.Width - maskOffset; x++)
                {
                    blue = green = red = 0.0;

                    offset = y * sourceData.Stride + x * 4;

                    for (int i = -maskOffset; i <= maskOffset; i++)
                    {
                        for (int j = -maskOffset; j <= maskOffset; j++)
                        {
                            imageOffset = offset + (j * 4) + (i * sourceData.Stride);
                            blue += (double)(originalByteData[imageOffset]) * filterMatrix.FilterMatrix[i + maskOffset, j + maskOffset];
                            green += (double)(originalByteData[imageOffset + 1]) * filterMatrix.FilterMatrix[i + maskOffset, j + maskOffset];
                            red += (double)(originalByteData[imageOffset + 2]) * filterMatrix.FilterMatrix[i + maskOffset, j + maskOffset];
                            
                        }
                    }

                    blue *= factor;
                    green *= factor;
                    red *= factor;
                    


                    newByteData[offset] = (byte)(originalByteData[offset] * oroginalCoeff + blue * coefficient);
                    newByteData[offset + 1] = (byte)(originalByteData[offset + 1] * oroginalCoeff + green * coefficient);
                    newByteData[offset + 2] = (byte)(originalByteData[offset + 2] * oroginalCoeff + red * coefficient);
                    newByteData[offset + 3] = 255;
                }
            }
            Marshal.Copy(newByteData, 0, resultData.Scan0, newByteData.Length);
            outputBitmap.UnlockBits(resultData);
            
            return outputBitmap;
        }

        
        private double maskAddition(double colorX, double colorY)
        {
            return Math.Abs(colorX) + Math.Abs(colorY);
        }
    }

}
