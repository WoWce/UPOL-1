using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_error_dithering
{
    class Matrix
    {
        private int[,] matrix;
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Matrix(int Height, int Width)
        {
            this.Height = Height;
            this.Width = Width;
            matrix = new int[Height, Width];
        }

        public void set(int m, int n, int value)
        {
            matrix[m, n] = value;
        }

        /// <summary>
        /// Gets value on specified position
        /// </summary>
        public int Get(int m, int n)
        {
            return matrix[m, n];
        }
        /// <summary>
        /// Gets size of the side of matrix
        /// </summary>
        public int GetWidth()
        {
            return Width;
        }

        public int GetHeight()
        {
            return Height;
        }
        /// <summary>
        /// Adds matrices
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="m2">Matrix</param>
        /// <returns>Matrix</returns>
        /*public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix result = new Matrix(m1.GetWidth());
            for(int i = 0; i < result.GetSize(); i++)
            {
                for (int j = 0; j < result.GetSize(); j++)
                {
                    result.set(i, j, m1.Get(i, j) + m2.Get(i, j));
                }
            }
            return result;
        }*/
        /// <summary>
        /// Multiplies matrix by number
        /// </summary>
        /// <param name="constant">Integer</param>
        /// <param name="m2">Matrix</param>
        /// <returns>Matrix</returns>
        /*public static Matrix operator *(int constant, Matrix m2)
        {
            Matrix result = new Matrix(m2.GetSize());
            for (int i = 0; i < result.GetSize(); i++)
            {
                for (int j = 0; j < result.GetSize(); j++)
                {
                    result.set(i, j, constant * m2.Get(i, j));
                }
            }
            return result;
        }*/

        public int this[int m, int n]
        {
            get
            {
                return Get(m, n);
            }
            set
            {
                matrix[m, n] = value;
            }
        }
        /// <summary>
        /// Is used to print matrix in console
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string outputMatrix = "";
            for (int i = 0; i < this.GetHeight(); i++)
            {
                for (int j = 0; j < this.GetWidth(); j++)
                {
                    outputMatrix += matrix[i, j];
                    outputMatrix += " ";
                }
                outputMatrix += "\n";
            }
            return outputMatrix;
        }

        /// <summary>
        /// Fills matrix with one number
        /// </summary>
        /// <param name="constant">Number to fill the matrix</param>
        public void FillWithConst(int constant)
        {
            for (int i = 0; i < this.GetHeight(); i++)
            {
                for (int j = 0; j < this.GetWidth(); j++)
                {
                    matrix[i, j] = constant;
                }
            }
        }
    }
}
