using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9_noise_reduction
{
    abstract class Filter
    {

        public abstract double Factor
        {
            get;
        }

        public abstract double[,] FilterMatrix
        {
            get;
        }
    }
}
