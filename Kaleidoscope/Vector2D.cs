using System;

namespace WpfApp
{
    public class Vector2D
    {
        public double x { get; set; }

        public double y { get; set; }

        public Vector2D(double x = 0.0, double y = 0.0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D CopyToVector()
        {
            return new Vector2D(x, y);
        }

        public double[] CopyToArray()
        {
            return new double[2] { x, y };
        }

        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D Rotate(double a)
        {
            Vector2D vector2D = new Vector2D();
            double num = HeadingRad() + a;
            double num2 = Mag();
            x = Math.Cos(num) * num2;
            y = Math.Sin(num) * num2;
            return CopyToVector();
        }

        public double HeadingRad()
        {
            return Math.Atan2(y, x);
        }

        public double HeadingDeg()
        {
            double num = HeadingRad();
            return (num >= 0.0) ? (num * 180.0 / Math.PI) : ((Math.PI * 2.0 + num) * 360.0 / (Math.PI * 2.0));
        }

        public double Mag()
        {
            return Math.Sqrt(MagSq());
        }

        public double MagSq()
        {
            double num = x;
            double num2 = y;
            return num * num + num2 * num2;
        }
    }
}
