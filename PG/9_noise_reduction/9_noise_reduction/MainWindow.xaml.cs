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

namespace _9_noise_reduction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap originalBitmap = null;
        private Bitmap currentStateBmp = null;

        private ImageEditor imageEditor;
        private Convolution imageSharper;
        private ImageBlur imageBlur;

        private SobelFilter sobelFilter;
        private SobelFilterR robelFilterRev;
        private RobertsFilter12 robertsFilter1;
        private RobertsFilter22 robertsFilter2;
        private ConvolutionFilter convolutionFilterFactor16;
        private ConvolutionFilter2 convolutionFilterFactor9;
        private LaplaceFilter laplaceFilter;
        private LaplaceFilter2 laplaceFilter2;


        double koefficient = 0.5;

        public MainWindow()
        {
            InitializeComponent();
            initializeFilters();
            Apply_Button.IsEnabled = false;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DisableSlider();
                originalBitmap = new Bitmap(openFileDialog.FileName);

                
                loadMainImage(originalBitmap);
                ListBox.Visibility = Visibility.Visible;
                Apply_Button.IsEnabled = true;
                ListBox.IsEnabled = true;
            }
        }

        private void loadMainImage(Bitmap bmp)
        {
            try
            {
                image1.Source = convertBmpToImage(bmp);
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

        private void initializeFilters()
        {
            imageEditor = new ImageEditor();
            imageSharper = new Convolution();
            imageBlur = new ImageBlur();

            //all these matrices are contained in FilterMatrices.cs
            sobelFilter = new SobelFilter();
            robelFilterRev = new SobelFilterR();
            robertsFilter1 = new RobertsFilter12();
            robertsFilter2 = new RobertsFilter22();
            convolutionFilterFactor16 = new ConvolutionFilter();
            convolutionFilterFactor9 = new ConvolutionFilter2();
            laplaceFilter = new LaplaceFilter();
            laplaceFilter2 = new LaplaceFilter2();
        }

        private void Conv1SelectedAction(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, convolutionFilterFactor16, 1.0, 0);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Conv2SelectedAction(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, convolutionFilterFactor9, 1.0, 0);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void MedianSelected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageBlur.ApplyMedianFilter(originalBitmap, 3);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void RobertsSelected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, robertsFilter1, robertsFilter2, koefficient);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableSlider();
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SobelSelected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, sobelFilter, sobelFilter, koefficient);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableSlider();
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LaplaceSelected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, laplaceFilter, koefficient);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableSlider();
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Laplace2Selected(object sender, RoutedEventArgs e)
        {

            DisableSlider();
            DisableList();
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Factory.StartNew(() =>
            {
                currentStateBmp = imageSharper.ApplyMask(originalBitmap, laplaceFilter2, koefficient);
            }).ContinueWith(taskState =>
            {
                loadMainImage(currentStateBmp);
                EnableSlider();
                EnableList();
                Mouse.OverrideCursor = null;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }

        private void original_Selected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            currentStateBmp = originalBitmap;
            loadMainImage(currentStateBmp);
        }

        private void gray_Selected(object sender, RoutedEventArgs e)
        {
            DisableSlider();
            currentStateBmp = imageEditor.ToGray(originalBitmap);
            loadMainImage(currentStateBmp);
        }

        private void DisableList()
        {
            ListBox.IsEnabled = false;
        }

        private void EnableList()
        {
            ListBox.IsEnabled = true;
        }

        private void EnableSlider()
        {
            slider.Value = koefficient;
            slider.Visibility = Visibility.Visible;
        }

        private void DisableSlider()
        {
            slider.Visibility = Visibility.Hidden;
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            koefficient = slider.Value;
            if (laplace2Item.IsSelected)
            {
                Laplace2Selected(sender, e);
            }
            else if (convItem.IsSelected)
            {
                Conv1SelectedAction(sender, e);
            }
            else if (conv2Item.IsSelected)
            {
                Conv2SelectedAction(sender, e);
            }
            else if (medianItem.IsSelected)
            {
                MedianSelected(sender, e);
            }
            else if (robertsItem.IsSelected)
            {
                RobertsSelected(sender, e);
            }
            else if (sobelItem.IsSelected)
            {
                SobelSelected(sender, e);
            }
            else if (laplaceItem.IsSelected)
            {
                LaplaceSelected(sender, e);
            }
            
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            originalBitmap = currentStateBmp;
        }
    }
}
