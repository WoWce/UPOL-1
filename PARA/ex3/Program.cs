using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ex3
{
    class Program
    {
        private static Barrier barrier;
        static bool[] array;
        static int[] count;
        static int[] old;
        static void sum(int i, int n)
        {
            int d = 1;
            count[i] = array[i] == true ? 1 : 0;
            barrier.SignalAndWait();
            while(d < n)
            {
                old[i] = count[i];
                barrier.SignalAndWait();
                if (i - d >= 0)
                {
                    count[i] += old[i - d];
                }
                barrier.SignalAndWait();
                d *= 2;
            }
        }

        //2
        static int[] arr2;
        static int[] result;
        static int[] old2;

        static void sum2(int i, int n)
        {
            var lap = 0;
            result[i] = arr2[i];
            barrier.SignalAndWait();
            bool swaped = false;
            while (lap < n)
            {
                old2[i] = result[i];
                barrier.SignalAndWait();
                if (lap % 2 == 0)
                {
                    if (i % 2 == 0 && (i + 1 < n) && old2[i] > old2[i + 1])
                    {
                        result[i] = old2[i + 1];
                        result[i + 1] = old2[i];
                    } 
                } else
                {
                    if (i % 2 != 0 && (i + 1 < n) && old2[i] > old2[i + 1])
                    {
                        result[i] = old2[i + 1];
                        result[i + 1] = old2[i];
                    }
                }
                barrier.SignalAndWait();
                lap++;
            }
        }

        static void Main(string[] args)
        {
            //3.1
            int n = 10;
            array = new bool[n];
            count = new int[n];
            old = new int[n];

            Random r = new Random();
            for(int i = 0; i < n; i++)
            {
                array[i] = r.Next(100) < 50 ? false : true;
            }

            barrier = new Barrier(n, (b) =>
            {
                //Console.WriteLine("Post-Phase action: phase={0}", b.CurrentPhaseNumber);
            });

            List<Thread> actions = new List<Thread>();
            for (int i = 0; i < n; i++)
            {
                var index = i;
                actions.Add(new Thread(() =>
                {
                    sum(index, n);
                }));
                
            }
            actions.ForEach(x => x.Start());
            actions.ForEach(x => x.Join());
            
            Console.WriteLine("1)\nNumber of true values = " + count[n - 1]);

            //3.2
            arr2 = new int[n];
            result = new int[n];
            old2 = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr2[i] = r.Next(100);
            }

            barrier = new Barrier(n, (b) =>
            {
                //Console.WriteLine("Phase {0}", b.CurrentPhaseNumber);
            });
            List<Thread> thrs = new List<Thread>();
            for (int i = 0; i < n; i++)
            {
                var index = i;
                thrs.Add(new Thread(() =>
                {
                    sum2(index, n);
                }));

            }
            thrs.ForEach(x => x.Start());
            thrs.ForEach(x => x.Join());

            Console.WriteLine("2)\nArray: ");
            foreach (var i in arr2)
            {
                Console.Write($"{i}, ");
            }
            Console.WriteLine("\nSorted Array: ");
            foreach (var i in result)
            {
                Console.Write($"{i}, ");
            }


        }
    }
}
