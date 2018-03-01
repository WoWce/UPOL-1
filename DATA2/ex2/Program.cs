using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class FunctionalDep
    {
        public List<int> set1 { get; set; }
        public List<int> set2 { get; set; }

        public FunctionalDep(List<int> a, List<int> b)
        {
            this.set1 = a;
            this.set2 = b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            List<int> abcdefgh = new List<int> { 1,2,3,4,5};

            List<int> a = new List<int> { 1 };
            List<int> ab = new List<int>{ 1,2 };
            List<int> bd = new List<int> { 2, 4 };
            List<int> c = new List<int> { 3 };
            List<int> ac = new List<int> { 1, 3 };
            List<int> fg = new List<int> { 6, 7 };
            List<int> e = new List<int> { 5 };
            List<int> fc = new List<int> { 6, 3 };
            List<int> cg = new List<int> { 3, 7 };
            List<int> gh = new List<int> { 7, 8};
            List<int> d = new List<int> { 4 };
            List<int> de = new List<int> { 4, 5 };
            List<int> dec = new List<int> { 4, 5, 3 };
            List<int> g = new List<int> { 7 };
            List<int> ae = new List<int> { 1, 5 };
            List<int> f = new List<int> { 6 };
            List<int> b = new List<int> { 2 };
            //FZ
            List<FunctionalDep> T = new List<FunctionalDep>();
            T.Add(new FunctionalDep(ab, c));
            T.Add(new FunctionalDep(ac, fg));
            T.Add(new FunctionalDep(e, fc));
            T.Add(new FunctionalDep(cg, gh));
            T.Add(new FunctionalDep(d, ab));
            T.Add(new FunctionalDep(g, ae));
            T.Add(new FunctionalDep(f, b));

            List<FunctionalDep> T1 = new List<FunctionalDep>();
            T1.Add(new FunctionalDep(ab, bd));
            T1.Add(new FunctionalDep(b, a));
            T1.Add(new FunctionalDep(de, dec));


            List<FunctionalDep> T2 = new List<FunctionalDep>();
            List<FunctionalDep> T3 = new List<FunctionalDep>();
            List<FunctionalDep> T4 = new List<FunctionalDep>();

            int setsCount = 0;
            //Vygenerujeme vsechny podmnoziny -> udelame uzavery -> vypiseme pevne body
            SubSetsOf<int>(abcdefgh).ToList().ForEach(i 
                => {
                    List<FunctionalDep> tmp = new List<FunctionalDep>(T1);
                    List<int> iList = new List<int>(i);
                    List<int> result = closure(tmp, iList);
                    result.Sort();
                    if(result.SequenceEqual(iList))
                    {
                        Console.Write("{");
                        result.ForEach(j => Console.Write($"{j},"));
                        Console.WriteLine("}");
                        setsCount++;
                    }
                    
                });
            Console.WriteLine($"Count of fixpoints: {setsCount} ;)");

            //testing closure
            /*List<int> MSet = new List<int>() { 1, 2 };
            List<int> result = closure(T, MSet);
            result.Sort();
            Console.WriteLine(result.Equals(MSet));
            result.ForEach(i => Console.Write($"{i}, "));*/
        }

        public static bool isSubset(List<int> subset, List<int> set)
        {
            bool found = false;
            for (int i = 0; i < subset.Count; i++)
            {
                for (int j = 0; j < set.Count; j++)
                {
                    if(subset[i] == set[j])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
                else
                {
                    found = false;
                }
            }
            return true;
        }
        

        public static IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source)
        {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

            var element = source.Take(1);

            var haveNots = SubSetsOf(source.Skip(1));
            var haves = haveNots.Select(set => element.Concat(set));

            return haves.Concat(haveNots);
        }
        

        public static List<int> closure(List<FunctionalDep> T, List<int> M)
        {
            
            bool stable = true;
            do
            {
                stable = true;
                for (int i = T.Count - 1; i >= 0; i--)
                {
                    if (isSubset(T[i].set1, M))
                    {
                        M = M.Union(T[i].set2).ToList();
                        stable = false;
                        T.RemoveAt(i);
                    }
                }

            } while (!stable);
            return M;
        }
    }

    
}
