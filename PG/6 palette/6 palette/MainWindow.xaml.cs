using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.ComponentModel;

namespace _6_palette
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        }

        private Bitmap originalBitmap;
        private Bitmap fixedPaletteBmp;
        private Bitmap adaptivePaletteBmp;
        private FixedPaletteConverter p = new FixedPaletteConverter();
        private byte[] paletteImgData;
        private byte[] adaptivePaletteData;
        private bool lolOpened = false;
        private bool apiOpened = false;
        private readonly BackgroundWorker worker = new BackgroundWorker();

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //cancel converting
                if (worker.IsBusy == true && worker.WorkerSupportsCancellation == true)
                {
                    worker.CancelAsync();
                }
                //default state of WPF elements
                Convert_Button.IsEnabled = false;
                ConversionBox.Visibility = Visibility.Hidden;
                //open 3-3-2 format image
                if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".lol")
                {
                    paletteImgData = File.ReadAllBytes(openFileDialog.FileName);
                    originalBitmap = p.convertDataToBitmap(paletteImgData);
                }
                //open adaptive palette image
                else if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".api")
                {
                    adaptivePaletteData = File.ReadAllBytes(openFileDialog.FileName);
                    AdaptivePaletteReader r = new AdaptivePaletteReader();
                    originalBitmap = r.convertDataToBitmap(adaptivePaletteData);
                }
                //open simple image
                else
                {
                    originalBitmap = new Bitmap(openFileDialog.FileName);
                    loadMainImage(originalBitmap);
                    Convert_Button.IsEnabled = true;
                }
                Save_Button.IsEnabled = true;
                loadMainImage(originalBitmap);
            }

        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //start background converting
            if (worker.IsBusy != true)
            {
                Convert_Button.IsEnabled = false;
                Save_Button.IsEnabled = false;
                Progress_Label.Visibility = Visibility.Visible;
                progressBar1.Visibility = Visibility.Visible;
                worker.RunWorkerAsync();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveImg = new SaveFileDialog();
            saveImg.Filter = "Constant Palette Image (*.lol)|*.lol|Adaptive Palette Image (*.api)|*.api|PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
            saveImg.DefaultExt = ".lol";
            if (saveImg.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(saveImg.FileName);
                switch (ext.ToLower())
                {
                    //save as 3-3-2 image
                    case ".lol":
                        try
                        {
                            if (paletteImgData == null) throw new NullReferenceException();
                            using (var fs = new FileStream(saveImg.FileName, FileMode.Create, FileAccess.Write))
                            {
                                fs.Write(paletteImgData, 0, paletteImgData.Length);
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            MessageBox.Show("Error while saving!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    //save as adaptive palette
                    case ".api":
                        try
                        {
                            if (adaptivePaletteData == null) throw new NullReferenceException();
                            using (var fs = new FileStream(saveImg.FileName, FileMode.Create, FileAccess.Write))
                            {
                                fs.Write(adaptivePaletteData, 0, paletteImgData.Length);
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            MessageBox.Show("Error while saving!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case ".jpg":
                        originalBitmap.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        originalBitmap.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        originalBitmap.Save(saveImg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(ext);
                }

                

            }

        }
        //show image in MainImage box
        private void loadMainImage(Bitmap loadBmp)
        {
            try
            {
                Img1.Source = convertBmpToImage(loadBmp);
            }
            catch (Exception exc)
            {
                MessageBoxResult res = MessageBox.Show(exc.Message, "Image Load Error", MessageBoxButton.OK);
            }
        }
        //show converted to 3-3-2 palette image
        private void loadFixedPaletteImg(Bitmap loadBmp)
        {
            try
            {
                FPalette.Source = convertBmpToImage(loadBmp);
            }
            catch (Exception exc)
            {
                MessageBoxResult res = MessageBox.Show(exc.Message, "Image Load Error", MessageBoxButton.OK);
            }
        }
        //show converted to adaptive palette image
        private void loadAdaptivePaletteImg(Bitmap loadBmp)
        {
            try
            {
                APalette.Source = convertBmpToImage(loadBmp);
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
            AdaptivePaletteConverter apl = new AdaptivePaletteConverter(originalBitmap);
            worker.ReportProgress(7);
            AdaptivePaletteReader apr = new AdaptivePaletteReader();
            worker.ReportProgress(14);
            adaptivePaletteData = apl.GetImageData();
            worker.ReportProgress(50);
            adaptivePaletteBmp = apr.convertDataToBitmap(adaptivePaletteData);
            worker.ReportProgress(70);
            paletteImgData = p.GetImageData(originalBitmap);
            worker.ReportProgress(82);
            fixedPaletteBmp = p.convertDataToBitmap(paletteImgData);
            worker.ReportProgress(100);
        }
        //update interface when completed
        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            Progress_Label.Visibility = Visibility.Hidden;
            progressBar1.Visibility = Visibility.Hidden;
            ConversionBox.Visibility = Visibility.Visible;
            loadFixedPaletteImg(fixedPaletteBmp);
            loadAdaptivePaletteImg(adaptivePaletteBmp);
            Save_Button.IsEnabled = true;
        }
        //progressbar changes
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

    }
}
