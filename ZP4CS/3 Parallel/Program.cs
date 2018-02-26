using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_Parallel
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] arr = GetRandomArray(1000000);
            int[] arr2 = new int[arr.Length];
            int[] arr3 = new int[arr.Length];
            Array.Copy(arr, arr2, arr.Length);
            Array.Copy(arr, arr3, arr.Length);

            Stopwatch sw = Stopwatch.StartNew();
            ParallelMergeSort(arr, 1);
            sw.Stop();
            Console.WriteLine("Sequential: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine(IsSorted(arr));

            sw = Stopwatch.StartNew();
            ParallelMergeSort(arr2, 2);
            sw.Stop();
            Console.WriteLine("Depth = 1: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine(IsSorted(arr2));

            sw = Stopwatch.StartNew();
            ParallelMergeSort(arr3, 3);
            sw.Stop();
            Console.WriteLine("Depth = 2: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine(IsSorted(arr3));
            
            Console.ReadLine();
        }

        public static int[] GetRandomArray(int size)
        {
            int[] arr = new int[size];
            Random randNum = new Random();
            for (int i = 0; i < size; i++)
            {
                arr[i] = randNum.Next(0, 1000);
            }
            return arr;
        }

        public static void ParallelMergeSort(int[] arr, int depth)
        {
            ParallelMergeSort sort = new ParallelMergeSort();
            sort.PMergeSort(arr, 0, arr.Length - 1, depth);
        }

        public static void PrintArray<T>(T[] t)
        {
            foreach (T item in t)
            {
                Console.Write($"{item}, ");
            }
            Console.WriteLine();
        }

        public static bool IsSorted(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
