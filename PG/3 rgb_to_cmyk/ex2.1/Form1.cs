using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ex2._1
{
    public partial class Form1 : Form
    {
        int R = 255;
        int G = 255;
        int B = 255;

        double C = 0;
        double M = 0;
        double Y = 0;
        double K = 0;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.BackColor = Color.FromArgb(R, G, B);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        //R
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            readRgbData();
            changeSquareColor();
        }

        //G
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            readRgbData();
            changeSquareColor();
        }

        //B
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            readRgbData();
            changeSquareColor();
        }

        //C
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            readCmykData();
            setRgbFromCmyk();
            changeSquareColor();
        }

        //M
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            readCmykData();
            setRgbFromCmyk();
            changeSquareColor();
        }

        //Y
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            readCmykData();
            setRgbFromCmyk();
            changeSquareColor();
        }

        //K
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            readCmykData();
            setRgbFromCmyk();
            changeSquareColor();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void changeSquareColor()
        {
            pictureBox1.BackColor = Color.FromArgb(R, G, B);
            showRgb();
        }

        private void showCmykFromRgb()
        {
            double J = Math.Max(Math.Max(R, G), B);
            double C = 1 - (double)this.R / J;
            double M = 1 - (double)this.G / J;
            double Y = 1 - (double)this.B / J;
            double K = 1 - J / 255;
            label14.Text = Math.Round(C, 2).ToString();
            label15.Text = Math.Round(M, 2).ToString();
            label16.Text = Math.Round(Y, 2).ToString();
            label17.Text = Math.Round(K, 2).ToString();
        }

        private void showCmyk()
        {
            label14.Text = C.ToString();
            label15.Text = M.ToString();
            label16.Text = Y.ToString();
            label17.Text = K.ToString();
        }

        private void showRgb()
         {
            label11.Text = R.ToString();
            label12.Text = G.ToString();
            label13.Text = B.ToString();
         }

        private void setRgbFromCmyk()
        {
            int twofivefive = 255;
            double R = twofivefive * (1 - this.C) * (1 - this.K);
            double G = twofivefive * (1 - this.M) * (1 - this.K);
            double B = twofivefive * (1 - this.Y) * (1 - this.K);
            this.R = (int)R;
            this.G = (int)G;
            this.B = (int)B;
        }

        private void readRgbData()
        {
            R = Decimal.ToInt32(numericUpDown1.Value);
            G = Decimal.ToInt32(numericUpDown2.Value);
            B = Decimal.ToInt32(numericUpDown3.Value);
            showCmykFromRgb();

        }

        private void readCmykData()
        {
            int hundred = 100;
            C = Decimal.ToDouble(numericUpDown4.Value) / hundred;
            M = Decimal.ToDouble(numericUpDown5.Value) / hundred;
            Y = Decimal.ToDouble(numericUpDown6.Value) / hundred;
            K = Decimal.ToDouble(numericUpDown7.Value) / hundred;
            showCmyk();
        }

        //Calculates optimal distance to pic
        private void distanceCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap b = new Bitmap(openFileDialog1.FileName);
                string dpiVal = InputDialog.ShowDialog("Enter DPI", "DPI");
                int distance = (int)(((double)b.Width / Int32.Parse(dpiVal)) * 2.54 / 2.0 / Math.Tan((double)b.Width / 60 / 2 * Math.PI / 180.0));
                MessageBox.Show("Optimal distance to picture is " + distance + " cm", "Distance Info");
            }
        }

        

    }
}
