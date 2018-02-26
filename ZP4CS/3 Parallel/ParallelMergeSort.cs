using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _3_Parallel
{
    class ParallelMergeSort
    {
        public void PMergeSort(int[] arr, int p, int r, int depth)
        {
            
            depth--;
            if (p < r && depth > 0)
            {
                int q = (p + r) / 2;
                Thread t1 = new Thread(() => PMergeSort(arr, p, q, depth));
                Thread t2 = new Thread(() => PMergeSort(arr, q+1, r, depth));
                t1.Start();
                t2.Start();
                t1.Join();
                t2.Join();

                Merge(arr, p, q, r);
            }
            else if(p < r)
            {
                int q = (p + r) / 2;
                SequentialMergeSort(arr, p, q);
                SequentialMergeSort(arr, q + 1, r);
                Merge(arr, p, q, r);
            }
        }

        private void Merge(int[] A, int p, int q, int r)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            int[] L = new int[n1 + 1];
            int[] R = new int[n2 + 1];
            Array.Copy(A, p, L, 0, L.Length - 1);
            Array.Copy(A, q + 1, R, 0, R.Length - 1);

            L[n1] = int.MaxValue;
            R[n2] = int.MaxValue;

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

        private void SequentialMergeSort(int[] arr, int l, int r)
        {

            if (l < r)
            {
                int q = (l + r) / 2;
                SequentialMergeSort(arr, l, q);
                SequentialMergeSort(arr, q + 1, r);
                Merge(arr, l, q, r);
            }
        }

    }
}
