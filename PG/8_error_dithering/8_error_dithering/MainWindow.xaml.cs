using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Bitmap bitmap1;
        Bitmap bitmap2;
        Bitmap bitmap3;
        Bitmap bitmap4;
        Bitmap bitmap5;
        Bitmap bitmap6;
        Bitmap bitmap7;
        Bitmap bitmap8;
        private readonly BackgroundWorker worker;
        Dithering dithering;
        Palette palette;

        public MainWindow()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
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
                
                if (worker.IsBusy != true)
                {
                    openBtn.IsEnabled = false;
                    label1.Visibility = Visibility.Hidden;
                    label2.Visibility = Visibility.Hidden;
                    label3.Visibility = Visibility.Hidden;
                    label4.Visibility = Visibility.Hidden;
                    progressBar.Visibility = Visibility.Visible;
                    worker.RunWorkerAsync();
                }
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(3);
            dithering = new Dithering();                        worker.ReportProgress(5);
            grayMap = dithering.SimpleDithering(bitmap);        worker.ReportProgress(8);
            palette = new Palette();                            worker.ReportProgress(10);
            bitmap1 = dithering.SimpleDithering(grayMap, 1);    worker.ReportProgress(18);
            bitmap2 = dithering.SimpleDithering(grayMap, 2);    worker.ReportProgress(26);
            bitmap3 = dithering.SimpleDithering(grayMap, 3);    worker.ReportProgress(36);
            bitmap4 = dithering.SimpleDithering(grayMap, 4);    worker.ReportProgress(48);
            bitmap5 = palette.correctImage(bitmap);             worker.ReportProgress(57);
            bitmap6 = palette.correctImage(bitmap, 1);          worker.ReportProgress(67);
            bitmap7 = palette.correctImage(bitmap, 2);          worker.ReportProgress(80);
            bitmap8 = palette.correctImage(bitmap, 3);          worker.ReportProgress(95);
            worker.ReportProgress(100);
        }

        //update interface when completed
        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            progressBar.Visibility = Visibility.Hidden;
            loadMainImage(bitmap1, bitmap2, bitmap3, bitmap4, bitmap5, bitmap6, bitmap7, bitmap8);
            openBtn.IsEnabled = true;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            label4.Visibility = Visibility.Visible;
        }

        //progressbar changes
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
