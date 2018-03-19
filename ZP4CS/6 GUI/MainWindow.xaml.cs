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

namespace _6_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path;
        string content;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = "*.txt";
                var result = dlg.ShowDialog();
                if(result == true)
                {
                    path = dlg.FileName;
                    PathLabel.Content = "File Path " + path;
                    content = File.ReadAllText(path);
                    TextBox.Text = content;
                    Label1.Content = "Symbols: " + content.Length;
                }
            }
            catch(Exception ex)
            {
                MessageBoxResult res = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox.Text.Length > 0)
            {
                try
                {
                    SaveFileDialog sdlg = new SaveFileDialog();
                    sdlg.DefaultExt = ".txt";
                    sdlg.Filter = "Txt (*.txt)|*.txt";
                    if (sdlg.ShowDialog() == true)
                        File.WriteAllText(sdlg.FileName, TextBox.Text);
                }
                catch (Exception exc)
                {
                    MessageBoxResult res = MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK);
                }
             
            }
        }
        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
             Label1.Content = "Symbols: " + TextBox.Text.Length;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TextBox.Text.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Want to Save?", "Confirming", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveFileDialog sdlg = new SaveFileDialog();
                        sdlg.DefaultExt = ".txt";
                        sdlg.Filter = "Txt (*.txt)|*.txt";
                        if (sdlg.ShowDialog() == true)
                            File.WriteAllText(sdlg.FileName, TextBox.Text);
                    }
                    catch (Exception exc)
                    {
                        MessageBoxResult res = MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK);
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Want to Save?", "Confirming", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveFileDialog sdlg = new SaveFileDialog();
                        sdlg.DefaultExt = ".txt";
                        sdlg.Filter = "Txt (*.txt)|*.txt";
                        if (sdlg.ShowDialog() == true)
                            File.WriteAllText(sdlg.FileName, TextBox.Text);
                    }
                    catch (Exception exc)
                    {
                        MessageBoxResult res = MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK);
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                }
        }
    }
}
