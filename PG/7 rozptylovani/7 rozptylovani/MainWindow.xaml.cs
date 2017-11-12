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
        Dithering ditheringProcessor;
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
                ditheringProcessor = new Dithering();
                bitmap = new Bitmap(openFileDialog.FileName);
                ditheringProcessor.ditheringMatrixSize = 16;
                originalButton.IsChecked = true;
                originalButton.IsEnabled = true;
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

       
        //background image convertation worker
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(3);
            Bitmap grayImageMap = ditheringProcessor.SimpleDithering(bitmap); worker.ReportProgress(5);
            var mapCopy1 = new Bitmap(grayImageMap); worker.ReportProgress(15);
            var mapCopy2 = new Bitmap(grayImageMap); worker.ReportProgress(25);
            var mapCopy3 = new Bitmap(grayImageMap); worker.ReportProgress(35);
            threshold = ditheringProcessor.SimpleDithering(mapCopy1, 1); worker.ReportProgress(50);
            random = ditheringProcessor.SimpleDithering(mapCopy2, 2); worker.ReportProgress(70);
            matrix = ditheringProcessor.MatrixDithering(mapCopy3); 
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
