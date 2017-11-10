using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7_rozptylovani
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap bitmap;
        private Bitmap threshold;
        private Bitmap random;
        private Bitmap matrix;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                bitmap = new Bitmap(openFileDialog.FileName);
                originalButton.IsChecked = true;
                loadMainImage(bitmap);
                if (worker.IsBusy != true)
                {
                    ThresholdBtn.IsEnabled = false;
                    RandomBtn.IsEnabled = false;
                    MatrixBtn.IsEnabled = false;
                    LoadingLabel.Visibility = Visibility.Visible;
                    LoadingBar.Visibility = Visibility.Visible;
                    worker.RunWorkerAsync();
                }
            }

        }

        private void Original_Checked(object sender, RoutedEventArgs e)
        {
            loadMainImage(bitmap);
        }

        private void Threshold_Checked(object sender, RoutedEventArgs e)
        {
            loadMainImage(threshold);
        }

        private void Random_Checked(object sender, RoutedEventArgs e)
        {
            loadMainImage(random);
        }

        private void Matrix_Checked(object sender, RoutedEventArgs e)
        {
            loadMainImage(matrix);
        }

        //loads image to show in main window
        private void loadMainImage(Bitmap loadBmp)
        {
            try
            {
                imageOriginal.Source = convertBmpToImage(loadBmp);
            }
            catch (Exception exc)
            {
                MessageBoxResult res = MessageBox.Show(exc.Message, "Image Load Error", MessageBoxButton.OK);
            }
        }

        private BitmapImage convertBmpToImage(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        //greyscale of 32bpp image with parameters to use dithering
        private Bitmap greyscale(Bitmap b, int optionChecked)
        {
            Bitmap bmp = b.Clone(new System.Drawing.Rectangle(0, 0, b.Width, b.Height), b.PixelFormat);
            //Lock bitmap's bits to system memory
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //Scan for the first line
            IntPtr ptr = bmpData.Scan0;

            //Declare an array in which your RGB values will be stored
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            //Copy RGB values in that array
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            //Create new bitmap for matrix dithering
             
            
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                //Set RGB values in a Array where all RGB values are stored
                byte gray = (byte)((double)(rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2]) / 3);
                switch (optionChecked)
                {
                    case 1:
                        gray = (byte)thresholdDithering(gray);
                        break;
                    case 2:
                        gray = (byte)randomDithering(gray);
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
                
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
            }
                    

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);
            rgbValues = null;
            bmpData = null;
            return bmp;

        }

        private int thresholdDithering(byte value)
        {
            
            if (value > 150)
            {
                return 255;
            }
            else return 0;
        }

        private int randomDithering(byte value)
        {
            
            int colorNumber = rnd.Next(0, 255);
            if (value > colorNumber)
            {
                return 255;
            }
            else return 0;
        }

        private Bitmap matrixDithering(Bitmap b)
        {
            Bitmap gray = greyscale(b, 3);
            Bitmap matrixBmp = new Bitmap(gray.Width * 16, gray.Height * 16);
            Matrix ditheringMatrix = matrixCount(16);
            BitmapData data = gray.LockBits(new System.Drawing.Rectangle(0, 0, gray.Width, gray.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

               
            for (int y = 0; y < data.Height; y++)
            {
                for (int x = 0; x < data.Width; x++)
                {
                    System.Drawing.Color PixelColor = System.Drawing.Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * y) + (4 * x)));
                    Matrix current = countDitheringMtx(PixelColor.R, ditheringMatrix);

                    for (int j = 0; j < current.size; j++)
                    {
                        for (int i = 0; i < current.size; i++)
                        {
                            matrixBmp.SetPixel(j + x * 16, i + y * 16, System.Drawing.Color.FromArgb(current[i, j], current[i, j], current[i, j]));
                        }
                    }
                    
                    
                }
            }
            gray.UnlockBits(data);
            return matrixBmp;
        }

        private Matrix countDitheringMtx(int inputValue, Matrix matrix)
        {
            Matrix result = new Matrix(matrix.size);
            for (int i = 0; i < matrix.size; i++)
            {
                for (int j = 0; j < matrix.size; j++)
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

        private Matrix matrixCount(int n)
        {
            Matrix m = new Matrix(2);
            m[0, 0] = 0;
            m[0, 1] = 2;
            m[1, 0] = 3;
            m[1, 1] = 1;
            Matrix result = createMatrix(m, n);
            return result;
        }


        private Matrix createMatrix(Matrix m, int n)
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
                return createMatrix(result, n);
            }
            else return m;
            
        }

        //background image convertation worker
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(3);
            threshold = greyscale(bitmap, 1);
            worker.ReportProgress(10);
            random = greyscale(bitmap, 2);
            worker.ReportProgress(20);
            matrix = matrixDithering(bitmap);
            worker.ReportProgress(100);
        }
        //update interface when completed
        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Hidden;
            LoadingLabel.Visibility = Visibility.Hidden;
            ThresholdBtn.IsEnabled = true;
            RandomBtn.IsEnabled = true;
            MatrixBtn.IsEnabled = true;
        }
        //progressbar changes
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingBar.Value = e.ProgressPercentage;
        }
    }
}
