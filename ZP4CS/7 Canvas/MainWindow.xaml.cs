using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace _7_WPF_Canvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();
        Line currentLine = new Line();
        Rectangle currentRect = new Rectangle();

        Polygon currentPoly = new Polygon();

        Ellipse currentCircle = new Ellipse();

        private bool line = true;
        private bool rectangle = false;
        private bool poly = false;
        private bool polygonActive = false;
        private bool ellipse = false;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void CanvasPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && poly)
            {
                polygonActive = false;
            } else if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(this);
                if (line)
                {
                    currentLine = new Line();
                    currentLine.Stroke = Brushes.Red;
                    currentLine.X1 = currentPoint.X;
                    currentLine.Y1 = currentPoint.Y;
                    currentLine.X2 = currentPoint.X;
                    currentLine.Y2 = currentPoint.Y;
                    currentLine.StrokeThickness = 4;
                    CanvasPanel.Children.Add(currentLine);
                } else if (rectangle)
                {
                    currentRect = new Rectangle();
                    currentRect.Height = 0;
                    currentRect.Width = 0;
                    //currentRect.Fill = Brushes.Blue;
                    currentRect.StrokeThickness = 3;
                    currentRect.Stroke = Brushes.DarkMagenta;
                    CanvasPanel.Children.Add(currentRect);
                    Canvas.SetLeft(currentRect, currentPoint.X);
                    Canvas.SetTop(currentRect, currentPoint.Y);
                } else if (poly && !polygonActive)
                {
                    currentPoly = new Polygon();
                    currentPoly.Stroke = Brushes.Cyan;
                    currentPoly.StrokeThickness = 3;
                    currentPoly.Points.Add(currentPoint);
                    currentPoly.Points.Add(currentPoint);
                    CanvasPanel.Children.Add(currentPoly);
                    polygonActive = true;
                } else if(poly && polygonActive)
                {
                    currentPoly.Points.Add(currentPoint);
                } else if (ellipse)
                {
                    currentCircle = new Ellipse();
                    currentCircle.Height = 0;
                    currentCircle.Width = 0;
                    currentCircle.Stroke = Brushes.DarkGoldenrod;
                    CanvasPanel.Children.Add(currentCircle);
                    Canvas.SetLeft(currentCircle, currentPoint.X);
                    Canvas.SetTop(currentCircle, currentPoint.Y);
                }
            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (line)
                {
                    currentLine.X2 = e.GetPosition(this).X;
                    currentLine.Y2 = e.GetPosition(this).Y;
                } else if (rectangle)
                {
                    currentRect.Width = Math.Abs(e.GetPosition(this).X - currentPoint.X);
                    currentRect.Height = Math.Abs(e.GetPosition(this).Y - currentPoint.Y);
                } else if (ellipse)
                {
                    currentCircle.Width = Math.Abs(e.GetPosition(this).X - currentPoint.X);
                    currentCircle.Height = Math.Abs(e.GetPosition(this).X - currentPoint.X);
                }
                
            }
            if (poly && currentPoly.Points.Count > 0 && polygonActive)
            {
                //segment.Points[1] = e.GetPosition(CanvasPanel);
                //segment.Stroke = Brushes.Blue;
                currentPoly.Points[currentPoly.Points.Count - 1] = e.GetPosition(CanvasPanel);
            }
        }

        private void CanvasPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (poly && currentPoly.Points.Count > 2)
            {
                currentPoly.Points.RemoveAt(currentPoly.Points.Count - 1);
            }
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            line = true;
            rectangle = poly = ellipse = polygonActive = false;
        }

        private void RectButton_Click(object sender, RoutedEventArgs e)
        {
            rectangle = true;
            line = poly = ellipse = polygonActive = false;
        }

        private void PolyButton_Click(object sender, RoutedEventArgs e)
        {
            poly = true;
            rectangle = line = ellipse = polygonActive = false;
        }

        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            ellipse = true;
            rectangle = poly = line = polygonActive = false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sdlg = new SaveFileDialog();
                sdlg.DefaultExt = ".png";
                sdlg.Filter = "PNG (*.png)|*.png";
                if (sdlg.ShowDialog() == true)
                {
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)CanvasPanel.RenderSize.Width,
                    (int)CanvasPanel.RenderSize.Height, 96, 96, PixelFormats.Default);
                    rtb.Render(CanvasPanel);
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    using (var fs = File.OpenWrite(sdlg.FileName))
                    {
                        encoder.Save(fs);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nepovedlo se uloˇzit soubor");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CanvasPanel.Children.Clear();
        }
    }
}
