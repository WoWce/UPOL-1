using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9_noise_reduction
{
    class RobertsFilter12 : Filter
    {

        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 0, 0, 0, },
                            { 0, 1, 0, },
                            { 0, 0, -1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class RobertsFilter22 : Filter
    {
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 0, 0, 0, },
                            { 0, 0, 1, },
                            { 0, -1, 0, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class SobelFilterR : Filter
    {
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 0, 1, 2, },
                            { -1, 0, 1, },
                            { -2, -1, 0, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class SobelFilter : Filter
    {
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { -2, -1, 0, },
                            { -1, 0, 1, },
                            { 0, 1, 2, } };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class ConvolutionFilter : Filter
    {
        private double factor = 1.0 / 16.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 1, 2, 1, },
                            { 2, 4, 2, },
                            { 1, 2, 1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class ConvolutionFilter2 : Filter
    {
        private double factor = 1.0 / 9.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 1, 1, 1, },
                            { 1, 1, 1, },
                            { 1, 1, 1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class LaplaceFilter : Filter
    {
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 0, 1, 0, },
                            { 1, -4, 1, },
                            { 0, 1, 0, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    class LaplaceFilter2 : Filter
    {
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double[,] filterMatrix =
            new double[,] { { 1, 1, 1, },
                            { 1, -8, 1, },
                            { 1, 1, 1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }
}
