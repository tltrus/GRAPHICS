using System;
using System.Windows;

namespace DrawingVisualApp
{
    internal static class Tools
    {
        public static double ToRadians(double degree)
        {
            return degree * Math.PI / 180.0;
        }

        public static double Lerp(double start, double stop, double amt)
        {
            return amt * (stop - start) + start;
        }

        public static Vector Lerp(this Vector v, Vector v1, Vector v2, double amt)
        {
            Vector vector1 = new Vector();
            vector1.X = v1.X;
            vector1.Y = v1.Y;

            Vector vector2 = new Vector();
            vector2.X = v2.X;
            vector2.Y = v2.Y;

            var x = (vector2.X - vector1.X) * amt;
            var y = (vector2.Y - vector1.Y) * amt;

            return new Vector(x, y);
        }
    }
}
