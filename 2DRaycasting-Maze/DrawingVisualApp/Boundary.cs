using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Boundary
    {
        public Vector2D a, b;

        public Boundary(double x1, double y1, double x2, double y2)
        {
            a = new Vector2D(x1, y1);
            b = new Vector2D(x2, y2);
        }

        public void Show(DrawingContext dc)
        {
            var p0 = new Point(a.x, a.y);
            var p1 = new Point(b.x, b.y);

            dc.DrawLine(new Pen(Brushes.White, 2), p0, p1);
        }
    }
}
