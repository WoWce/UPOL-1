using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Bitmap b;
        private Bitmap resultBmp;

        private Bitmap desatur;
        private Bitmap resultBmpD;

        private double c1 = 0.299;
        private double c2 = 0.587;
        private double c3 = 0.144;
        private double s = 1;

        private void LoadImage(Bitmap loadBmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                loadBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                Img1.Source = image;
            }
            catch (Exception exc)
            {
                MessageBoxResult res = MessageBox.Show(exc.Message, "Image Load Error", MessageBoxButton.OK);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                b = new Bitmap(openFileDialog.FileName);
                resultBmp = greyScale(b, 2);
                resultBmp = greyScale(b, 1);
                AverageRB.IsChecked = true;
                AverageRB.IsEnabled = true;
                BAverageRB.IsEnabled = true;
                SaveBtn.IsEnabled = true;
            }
        }

        private void Average_Checked(object sender, RoutedEventArgs e)
        {
            resultBmp = greyScale(b, 2);
            LoadImage(resultBmp);
            RSlider.IsEnabled = false;
            GSlider.IsEnabled = false;
            BSlider.IsEnabled = false;
        }

        private void BoundedAv_Checked(object sender, RoutedEventArgs e)
        {
            RSlider.IsEnabled = true;
            GSlider.IsEnabled = true;
            BSlider.IsEnabled = true;
            RSlider.Value = c1;
            GSlider.Value = c2;
            BSlider.Value = c3;
            resultBmp = greyScale(b, 1);
            LoadImage(resultBmp);
        }

        

        private void RSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            c1 = RSlider.Value;
            RLbl.Content = c1.ToString("n2");
            resultBmp = greyScale(b, 1);
            LoadImage(resultBmp);
        }

        private void GSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            c2 = GSlider.Value;
            GLbl.Content = c2.ToString("n2");
            resultBmp = greyScale(b, 1);
            LoadImage(resultBmp);
        }

        private void BSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            c3 = BSlider.Value;
            BLbl.Content = c3.ToString("n2");
            resultBmp = greyScale(b, 1);
            LoadImage(resultBmp);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            saveFile(resultBmp);
        }


        //******************* DESATURATION *******************

        private void SSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            s = SSlider.Value;
            SLbl.Content = s.ToString("n2");
            resultBmpD = greyScale(desatur, 3);
            LoadImage2(resultBmpD);
        }

        private void LoadImage2(Bitmap loadBmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                loadBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                Img2.Source = image;
            }
            catch (Exception exc)
            {
                MessageBoxResult res = MessageBox.Show(exc.Message, "Image Load Error", MessageBoxButton.OK);
            }
        }

        private void OpenD_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                desatur = new Bitmap(openFileDialog.FileName);
                SSlider.IsEnabled = true;
                s = 1;
                SSlider.Value = s;
                Save2.IsEnabled = true;
                LoadImage2(desatur);
            }
        }

        private void Save2_Click(object sender, RoutedEventArgs e)
        {
            saveFile(resultBmpD);
        }

        /*
         * helping methods 
         */


        private Bitmap greyScale(Bitmap b, int param)
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
            switch (param)
            {
                case 1:
                    for (int i = 0; i < rgbValues.Length; i += 3)
                    {
                        //Set RGB values in a Array where all RGB values are stored
                        byte gray = (byte)(rgbValues[i] * c1 + rgbValues[i + 1] * c2 + rgbValues[i + 2] * c3);
                        rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
                    }
                    break;
                case 2:
                    for (int i = 0; i < rgbValues.Length; i += 3)
                    {
                        //Set RGB values in a Array where all RGB values are stored
                        byte gray = (byte)((double)(rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2]) / 3);
                        rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
                    }
                    break;
                case 3:
                    for (int i = 0; i < rgbValues.Length; i += 3)
                    {
                        //Set RGB values in a Array where all RGB values are stored
                        byte gray = (byte)((rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2]) / 3);
                        rgbValues[i] = (byte)(gray * (1 - s) + rgbValues[i] * s);
                        rgbValues[i + 1] = (byte)(gray * (1 - s) + rgbValues[i + 1] * s);
                        rgbValues[i + 2] = (byte)(gray * (1 - s) + rgbValues[i + 2] * s);
                    }
                    break;
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);
            rgbValues = null;
            bmpData = null;
            return bmp;

        }
        
        private void saveFile(Bitmap result)
        {
            SaveFileDialog saveImg = new SaveFileDialog();
            saveImg.DefaultExt = "jpg";
            saveImg.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|All files(*.*)|*.*";

            if (saveImg.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(saveImg.FileName);

                switch (ext.ToLower())
                {
                    case ".jpg":
                        result.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        result.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        result.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(ext);
                }
            }
        }

    }
}
