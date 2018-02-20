using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Tuple
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr1 = { 5, 8, 2, 3 };
            int[] arr2 = { 3, 7, 9, 5 };
            int[] arr3 = { 7, 5, 9, 8 };

            Tuple[] tuples = { new Tuple(arr1), new Tuple(arr2), new Tuple(arr3) };
            PrintArray(tuples);
            MergeSort(tuples);
            //PrintArray(tuples);
            CMergeSort(tuples);
            PrintArray(tuples);
            
            int[] arr4 = { 7, 6, 1, 3 };
            int[] arr5 = { 4, 3, 1, 2 };

            PrintArray(MapMathOperation(arr4, arr5, Plus));
            PrintArray(MapMathOperation(arr4, arr5, Mult));

            Console.ReadLine();
        }

        delegate int MathOperation(int x, int y);

        static int Plus(int x, int y)
        {
            return x + y;
        }

        static int Mult(int x, int y)
        {
            return x * y;
        }

        static int[] MapMathOperation(int[] arr1, int[] arr2, MathOperation m)
        {
            int[] result = new int[arr1.Length];
            for(int i = 0; i < arr1.Length; i++)
            {
                result[i] = m(arr1[i], arr2[i]);
            }
            return result;
        }

        private static void PrintArray<T>(T[] a)
        {
            foreach(T t in a)
            {
                Console.Write($"{t}, ");
            }
            Console.WriteLine();
        }

        

        static void MergeSort(Tuple[] t)
        {
            Sort(t, 0, t.Length - 1);
        }

        private static void Sort(Tuple[] arr, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                
                Sort(arr, l, m);
                Sort(arr, m + 1, r);
                
                Merge(arr, l, m, r);
            }
        }

        static void Merge(Tuple[] A, int p, int q, int r)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            Tuple[] L = new Tuple[n1 + 1];
            Tuple[] R = new Tuple[n2 + 1];
            Array.Copy(A, p, L, 0, L.Length - 1);
            Array.Copy(A, q + 1, R, 0, R.Length - 1);

            int[] maxArr = new int[A[0].Size()];

            for (int i = 0; i < A[0].Size(); i++)
            {
                maxArr[i] = int.MaxValue;
            }

            L[n1] = new Tuple(maxArr);
            R[n2] = new Tuple(maxArr);
            int n = 0;
            int m = 0;
            for (int k = p; k <= r; k++)
            {
                if (L[n] <= R[m])
                {
                    A[k] = L[n];
                    n++;
                }
                else
                {
                    A[k] = R[m];
                    m++;
                }
            }
        }

        static void CMergeSort(Tuple[] t)
        {
            CSort(t, 0, t.Length - 1);
        }

        private static void CSort(IComparable[] arr, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                
                CSort(arr, l, m);
                CSort(arr, m + 1, r);
                
                CMerge(arr, l, m, r);
            }
        }

        static void CMerge(IComparable[] A, int p, int q, int r)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            IComparable[] L = new IComparable[n1 + 1];
            IComparable[] R = new IComparable[n2 + 1];
            Array.Copy(A, p, L, 0, L.Length - 1);
            Array.Copy(A, q + 1, R, 0, R.Length - 1);

            if (A[0] is Tuple)
            {
                int[] maxArr = new int[A.Length];
                for (int i = 0; i < A.Length; i++)
                {
                    maxArr[i] = int.MaxValue;
                }
                L[n1] = new Tuple(maxArr);
                R[n2] = new Tuple(maxArr);
            }
            else
            {
                L[n1] = int.MaxValue;
                R[n2] = int.MaxValue;
            }

            int n = 0;
            int m = 0;
            for (int k = p; k <= r; k++)
            {
                if (L[n].CompareTo(R[m]) < 0 || L[n].CompareTo(R[m]) == 0)
                {
                    A[k] = L[n];
                    n++;
                }
                else
                {
                    A[k] = R[m];
                    m++;
                }
            }
        }

    }
    
class Tuple : IComparable
    {
        private int[] tpl { get; set; }

        public int Size()
        {
            return tpl.Length;
        }

        public int CompareTo(object obj)
        {
            if (obj is Tuple)
            {
                if (this == (Tuple)obj) return 0;
                else if ((Tuple)obj < this) return 1;
                else return -1;
            }
            else
            {
                throw new Exception("Object is not a Tuple");
            }
        }

        public Tuple(int [] arr)
        {
            tpl = arr;
        }

        public static bool operator ==(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] != b.tpl[i]) return false;
            }
            return true;
        }

        public static bool operator !=(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] == b.tpl[i]) return false;
            }
            return true;
        }

        public static bool operator >(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] > b.tpl[i]) return true;
                else if (a.tpl[i] < b.tpl[i]) return false;
            }
            return false;
        }

        public static bool operator <(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] < b.tpl[i]) return true;
                else if (a.tpl[i] > b.tpl[i]) return false;
            }
            return false;
        }

        public static bool operator >=(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] > b.tpl[i]) return true;
                else if (a.tpl[i] < b.tpl[i]) return false;
            }
            return true;
        }

        public static bool operator <=(Tuple a, Tuple b)
        {
            for (int i = 0; i < a.Size(); i++)
            {
                if (a.tpl[i] < b.tpl[i]) return true;
                else if (a.tpl[i] > b.tpl[i]) return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tpl.Length; i++)
            {
                builder.Append(tpl[i]).Append(" ");
            }
            return builder.ToString();
        }
    }
}
