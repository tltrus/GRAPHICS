using System;
using System.Threading.Tasks;

namespace WpfApp
{
    static class Tools
    {
        public static double Constrain(double n, double low, double high)
        {
            return Math.Max(Math.Min(n, high), low);
        }

        public static double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            var newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return newval;
            }
            if (start2 < stop2)
            {
                return Constrain(newval, start2, stop2);
            }
            else
            {
                return Constrain(newval, stop2, start2);
            }
        }

        public static double NextDouble(this Random RandGenerator, double MinValue, double MaxValue)
        {
            return RandGenerator.NextDouble() * (MaxValue - MinValue) + MinValue;
        }

        public static void Swap(float[,] a, float[,] b, int rows, int cols)
        {
            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; j++)
                {
                    float tmp = a[i, j];
                    a[i, j] = b[i, j];
                    b[i, j] = tmp;
                }
            }); // Parallel.For
        }
    }
}
