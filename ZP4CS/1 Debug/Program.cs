using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Debug
{
    class Program
    {
        private static void MergeSort(int[] arr, int l, int r)
        {
#if DEBUG
            Console.WriteLine(Properties.Settings.Default.DEBUG);
#endif

            for (int i = 0; i < arr.Length - 1; i++)
            {
                Console.Write(arr[i] + ", ");
            }
            Console.WriteLine();
            Sort(arr, l, r);
        }

        private static void Sort(int[] arr, int l, int r)
        {
            if (l < r)
            {
                // Find the middle point
                int m = l + (r - l) / 2;

                // Sort first and second halves
                Sort(arr, l, m);
                Sort(arr, m + 1, r);

                // Merge the sorted halves
                merge(arr, l, m, r);
            }
        }

        private static void merge(int[] arr, int l, int m, int r)
        {
            // Find sizes of two subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;

            /* Create temp arrays */
            int[] L = new int[n1]; //would it change values than
            int[] R = new int[n2];

            /*Copy data to temp arrays*/
            //for (int a = 0; a < n1; ++a)
            //   L[a] = arr[l + a];
            Array.Copy(arr, l, L, 0, n1);
            //for (int b = 0; b < n2; ++b)
            //   R[b] = arr[m + 1 + b];
            Array.Copy(arr, m + 1, R, 0, n2);

            /* Merge the temp arrays */

            // Initial indexes of first and second subarrays
            int i = 0, j = 0;

            // Initial index of merged subarry array
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }

            /* Copy remaining elements of L[] if any */
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            /* Copy remaining elements of R[] if any */
            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }


        static void Main(string[] args)
        {
            int[] pole = { 4, 2, 7, 1, 8, 3, 6, 9, 5 };
            MergeSort(pole, 0, pole.Length - 1);
            for (int i = 0; i < pole.Length - 1; i++)
            {
                Console.Write(pole[i] + ", ");
            }
            Console.WriteLine();
        }
    }
}
