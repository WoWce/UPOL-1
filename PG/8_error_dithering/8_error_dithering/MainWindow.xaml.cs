using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

namespace _8_error_dithering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap;
        Bitmap grayMap;
        Bitmap bitmap3;
        Bitmap bitmap4;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                bitmap = new Bitmap(openFileDialog.FileName);
                Dithering dithering = new Dithering();
                grayMap = dithering.SimpleDithering(bitmap);
                Palette palette = new Palette();
                loadMainImage(dithering.SimpleDithering(grayMap, 1), 
                    dithering.SimpleDithering(grayMap, 2), 
                    dithering.SimpleDithering(grayMap, 3), 
                    dithering.SimpleDithering(grayMap, 4),
                    palette.correctImage(bitmap), 
                    palette.correctImage(bitmap, 1), 
                    palette.correctImage(bitmap, 2), 
                    palette.correctImage(bitmap, 3));


            }
        }

        private void loadMainImage(Bitmap bmp1, Bitmap bmp2, Bitmap bmp3, Bitmap bmp4, Bitmap bmp5, Bitmap bmp6, Bitmap bmp7, Bitmap bmp8)
        {
            try
            {
                imageOriginal.Source = convertBmpToImage(bmp1);
                image2.Source = convertBmpToImage(bmp2);
                image3.Source = convertBmpToImage(bmp3);
                image4.Source = convertBmpToImage(bmp4);
                image5.Source = convertBmpToImage(bmp5);
                image6.Source = convertBmpToImage(bmp6);
                image7.Source = convertBmpToImage(bmp7);
                image8.Source = convertBmpToImage(bmp8);
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
    }
}
