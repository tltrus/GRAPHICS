using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace DrawingVisualApp
{
    internal class DeCasteljo
    {
        public List<Point> points;
        Brush brush;

        public DeCasteljo(List<Point> points)
        {
            brush = Brushes.White;
            this.points = points;
        }

        private int Fuctorial(int n) // Функция вычисления факториала
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }

        private double Polinom(int k, int n, double t)
        {
            // многочлены Бернштейна — это алгебраические многочлены, представляющие собой линейную комбинацию базисных многочленов Бернштейна
            // (n + 1) базисных многочленов Бернштейна степени n находятся по формуле:
            // b<kn>(x) = (n / k) * x^k * (1 - x)^n-k, где k = 0, ... , n
            return (Fuctorial(n) / (Fuctorial(k) * Fuctorial(n - k))) * (float)Math.Pow(t, k) * (float)Math.Pow(1 - t, n - k);
        }

        public void Draw(DrawingContext dc)
        {
            int j = 0;

            List<Point> curve_points = new List<Point>();
            for (double t = 0; t < 1; t += 0.01)
            {
                double ytmp = 0;
                double xtmp = 0;
                for (int i = 0; i < points.Count; i++)
                {   // проходим по каждой точке 
                    // и для каждого шага t у нас будет одна общая усредненная точка из, например 7 исходных точек 
                    double b = Polinom(i, points.Count - 1, t);
                    xtmp += points[i].X * b; 
                    ytmp += points[i].Y * b;
                }
                curve_points.Add(new Point(xtmp, ytmp));
                dc.DrawEllipse(Brushes.DeepPink, null, curve_points[j], 1, 1);
                

                j++;
            }

            DrawLines(dc, curve_points);
        }

        private void DrawLines(DrawingContext dc, List<Point> points)
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], false, false);
                geometryContext.PolyLineTo(points, true, true);
            }

            dc.DrawGeometry(Brushes.DeepPink, new Pen(Brushes.DeepPink, 2), streamGeometry);
        }
    }
}
