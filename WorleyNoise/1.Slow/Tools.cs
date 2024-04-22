using System;

namespace WpfApp
{
    internal class Tools
    {
        public static double Constrain(double n, double low, double high)
        {
            return Math.Max(Math.Min(n, high), low);
        }

        public static double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            double num = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return num;
            }

            if (start2 < stop2)
            {
                return Constrain(num, start2, stop2);
            }

            return Constrain(num, stop2, start2);
        }
    }
}
