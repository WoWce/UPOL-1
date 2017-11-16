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
        Bitmap bitmap2;
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
                bitmap2 = new Bitmap(openFileDialog.FileName);
                bitmap3 = new Bitmap(openFileDialog.FileName);
                /*Dithering dithering = new Dithering();
                bitmap = dithering.SimpleDithering(bitmap);
                bitmap2 = dithering.SimpleDithering(bitmap);
                bitmap3 = dithering.SimpleDithering(bitmap);
                bitmap4 = dithering.SimpleDithering(bitmap);
                loadMainImage(dithering.SimpleDithering(bitmap, 1), dithering.SimpleDithering(bitmap2, 2), dithering.SimpleDithering(bitmap3, 3), dithering.SimpleDithering(bitmap4, 4));*/
                Palette palette = new Palette();
                loadMainImage(palette.SimpleDithering(bitmap), palette.SimpleDithering(bitmap2, 1), palette.SimpleDithering(bitmap3, 2));


            }
        }

        private void loadMainImage(Bitmap loadBmp, Bitmap bmp2, Bitmap bmp3/*, Bitmap bmp4*/)
        {
            try
            {
                imageOriginal.Source = convertBmpToImage(loadBmp);
                image2.Source = convertBmpToImage(bmp2);
                image3.Source = convertBmpToImage(bmp3);
                /*image4.Source = convertBmpToImage(bmp4);*/
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
