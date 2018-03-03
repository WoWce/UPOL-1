using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex2
{
    class FunctionalDep
    {
        public List<string> set1 { get; set; }
        public List<string> set2 { get; set; }

        public FunctionalDep(List<string> a, List<string> b)
        {
            this.set1 = a;
            this.set2 = b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> setOfAttributes = new List<string>{ "ID", "Jmeno", "Prijmeni", "Narozen", "Vyska" };
            List<string> tuple1 = new List<string> { "1", "Jan", "Spaleny", "1981", "Vysoky" };
            List<string> tuple2 = new List<string> { "2", "Jakub", "Spaleny", "1970", "Vysoky" };
            List<string> tuple3 = new List<string> { "3", "Jakub", "Mazany", "1970", "Vysoky" };
            List<string> tuple4 = new List<string> { "4", "Jan", "Spaleny", "1980", "Vysoky" };
            List<string> tuple5 = new List<string> { "5", "Karel", "Karel", "1971", "Maly" };
            List<List<string>> relation = new List<List<string>> { tuple1, tuple2, tuple3, tuple4, tuple5 };

            int setsCount = 0;
            int generalCount = 0;
            List<IEnumerable<string>> subattr =  SubSetsOf(setOfAttributes).ToList();
            bool forall = false;
            subattr.ForEach(i =>
            {
                subattr.ForEach(j =>
                {
                    if(i.Any() && j.Any())
                    {
                        if(IsDependencyInRelation(setOfAttributes, i.ToList(), j.ToList(), relation))
                        {
                            setsCount++;
                        }
                    }
                    
                });
            });
            Console.WriteLine(setsCount);
            Console.WriteLine(TwoTupleDependency(setOfAttributes, new List<string> { "Prijmeni", "Jmeno", "Narozen" }, new List<string> { "ID" }, tuple1, tuple4));
            
        }

        public static bool IsDependencyInRelation(List<string> attributes, List<string> FDLeft, List<string> FDRight, List<List<string>> relation)
        {
            bool forall = true;
            for (int k = 0; k < relation.Count; k++)
            {
                for (int l = k + 1; l < relation.Count; l++)
                {

                    if(TwoTupleDependency(attributes, FDLeft, FDRight, relation[k], relation[l]))
                    {
                        forall = true;
                    }
                    else
                    {
                        return false;
                    }
                            
                }
            }
            return forall;
        }

        public static bool TwoTupleDependency(List<string> attributes, List<string> FDLeft, List<string> FDRight, List<string> tuple1, List<string> tuple2)
        {
            
            bool equal = true;
            foreach(string s in FDLeft)
            {
                
                for (int i = 0; i < attributes.Count; i++)
                {
                    if (s.Equals(attributes[i]))
                    {
                        if (tuple1[i].Equals(tuple2[i]))
                        {
                            equal = true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    
                }
            }
            if (equal)
            {
                foreach (string s in FDRight)
                {
                    for (int i = 0; i < attributes.Count; i++)
                    {
                        if (s.Equals(attributes[i]))
                        {
                            if (tuple1[i].Equals(tuple2[i]))
                            {
                                equal = true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                    
                }
            }
            return equal;
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
    }
}
