using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class Bezier
    {
        public List<Point> points;
        Brush brush;
        enum Type
        {
            Quadratic,
            Cubic
        }
        Type type;

        public Bezier(List<Point> points)
        {
            brush = Brushes.White;
            this.points = points;

            switch (points.Count)
            {
                case 3: 
                    type = Type.Quadratic; 
                    break;
                case 4: 
                    type = Type.Cubic;
                    break;
            }
        }

        private Point QuadraticCalculation(double t)
        {
            double c0 = (1 - t) * (1 - t);
            double c1 = (1 - t) * 2 * t;
            double c2 = t * t;
            double x = c0 * points[0].X + c1 * points[1].X + c2 * points[2].X;
            double y = c0 * points[0].Y + c1 * points[1].Y + c2 * points[2].Y;
            return new Point(x, y);
        }
        private Point CubicCalculation(double t)
        {
            double c0 = (1 - t) * (1 - t) * (1 - t);
            double c1 = (1 - t) * (1 - t) * 3 * t;
            double c2 = (1 - t) * t * t * 3;
            double c3 = t * t * t;
            double x = c0 * points[0].X + c1 * points[1].X + c2 * points[2].X + c3 * points[3].X;
            double y = c0 * points[0].Y + c1 * points[1].Y + c2 * points[2].Y + c3 * points[3].Y;
            return new Point(x, y);
        }

        public void Draw(DrawingContext dc)
        {
            for (double t = 0; t < 1; t += 0.001)
            {
                Point p = new Point(0, 0);
                switch (type)
                {
                    case Type.Quadratic:
                        p = QuadraticCalculation(t);
                        break;
                    case Type.Cubic:
                        p = CubicCalculation(t);
                        break;
                }
                dc.DrawEllipse(Brushes.Lime, null, p, 1, 1);
            }
        }
    }
}
