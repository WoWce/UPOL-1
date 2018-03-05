using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_Files
{
    class Program
    {
        static void MergeSort(int[] A, int p, int r)
        {
            if (p < r)
            {
                int q = (p + r) / 2;
                MergeSort(A, p, q);
                MergeSort(A, q + 1, r);
                Merge(A, p, q, r);
            }
        }

        static void Merge(int[] A, int p, int q, int r)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            int[] L = new int[n1 + 1];
            int[] R = new int[n2 + 1];
            for (int i = 0; i < n1; i++)
            {
                L[i] = A[p + i];
            }
            for (int j = 0; j < n2; j++)
            {
                R[j] = A[q + j + 1];
            }
            L[n1] = int.MaxValue;
            R[n2] = int.MaxValue;
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

        static void FileMergeSort(string path, string writePath)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                StreamReader sr = null;
                List<int> array = new List<int>();
                try
                {
                    sr = fileInfo.OpenText();
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        char[] delimiterChars = { ' ', ',', '.' };
                        string[] numbers = s.Split(delimiterChars);
                        foreach (var it in numbers)
                        {
                            array.Add(int.Parse(it));
                        }
                    }

                    int[] arr = array.ToArray();
                    Console.WriteLine($"Nacteno pole ze souboru {path} velikosti {arr.Length}");
                    MergeSort(arr, 0, array.Count - 1);
                    WriteFile(arr, writePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Chyba pri cteni souboru");
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }

                }
            }
        }

        static void WriteFile(int[] arr, string path)
        {
            
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(File.Create(path));
                for (int i = 0; i < arr.Length; i++)
                {
                    sw.WriteLine(arr[i] + " ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Zapis souboru selhal");
            }
            finally
            {
                Console.WriteLine($"Zapsano pole do souboru {path}");
                if (sw != null)
                {
                    sw.Close();
                }

            }
        }

        static void Main(string[] args)
        {
            CustomConsole myConsole = new CustomConsole();
            myConsole.ReadCommand();
            string path = @"E:\test.txt";
            string writePath = @"E:\outPole.txt";
            FileMergeSort(path, writePath);
            Console.ReadLine();
        }
    }
}
