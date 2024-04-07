using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Ray
    {
        public Vector2D pos;
        public Vector2D dir;

        public Ray(Vector2D pos, double angle)
        {
            this.pos = pos;

            dir = Vector2D.FromAngle(angle);
            dir.Add(pos);
        }

        public Vector2D Cast(Boundary wall)
        {
            var x1 = wall.a.X;
            var y1 = wall.a.Y;
            var x2 = wall.b.X;
            var y2 = wall.b.Y;

            var x3 = pos.X;
            var y3 = pos.Y;
            var x4 = dir.X;
            var y4 = dir.Y;

            var den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
            {
                // если прямые параллельны
                return null;
            }

            var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;
            if (t > 0 && t < 1 && u > 0)
            {
                var pt = new Vector2D();
                pt.X = x1 + t * (x2 - x1);
                pt.Y = y1 + t * (y2 - y1);
                return pt;
            }
            else
            {
                return null;
            }
        }

        public void Show(DrawingContext dc)
        {
            var p0 = new Point(pos.X, pos.Y);
            var p1 = new Point(dir.X, dir.Y);
            dc.DrawLine(new Pen(Brushes.White, 1), p0, p1);
        }
    }
}
