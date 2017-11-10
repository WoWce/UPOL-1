using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_rozptylovani
{
    /*
     * Representation of matrix
     * To show matrix in console use Console.Write(matrix)
     */
    class Matrix
    {
        private int[,] matrix;
        public int size { get; private set; }

        public Matrix(int size)
        {
            this.size = size;
            matrix = new int[size, size];
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
        public int GetSize()
        {
            return size;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix result = new Matrix(m1.GetSize());
            for(int i = 0; i < result.GetSize(); i++)
            {
                for (int j = 0; j < result.GetSize(); j++)
                {
                    result.set(i, j, m1.Get(i, j) + m2.Get(i, j));
                }
            }
            return result;
        }

        public static Matrix operator *(int constant, Matrix m2)
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
        }

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

        public override string ToString()
        {
            string outputMatrix = "";
            for (int i = 0; i < this.GetSize(); i++)
            {
                for (int j = 0; j < this.GetSize(); j++)
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
            for (int i = 0; i < this.GetSize(); i++)
            {
                for (int j = 0; j < this.GetSize(); j++)
                {
                    matrix[i, j] = constant;
                }
            }
        }
    }
}
