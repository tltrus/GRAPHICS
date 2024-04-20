using System;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Star
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double pz { get; set; }

        public Star(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            pz = z;
        }

        public void Update()
        {
            z = z - MainWindow.speed;
            if (z < 1)
            {
                z = MainWindow.width;
                x = MainWindow.rnd.Next(-MainWindow.width, MainWindow.width);
                y = MainWindow.rnd.Next(-MainWindow.height, MainWindow.height);
                pz = z;
            }
        }

        public void Show(DrawingContext dc)
        {
            var sx = Map(x / z, 0, 1, 0, MainWindow.width) + MainWindow.offsX;
            var sy = Map(y / z, 0, 1, 0, MainWindow.height) + MainWindow.offsY;
            var r = Map(z, 0, MainWindow.width, 16, 0); ;

            dc.DrawEllipse(Brushes.WhiteSmoke, null, new Point(sx, sy), r, r);
        }

        public static double Constrain(double n, double low, double high) => Math.Max(Math.Min(n, high), low);
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
    }
}
