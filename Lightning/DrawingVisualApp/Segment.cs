using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MiscUtils.Vectors;

namespace DrawingVisualApp
{
    class Segment
    {
        public Vector2D a, b;
        double thickness;


        public Segment(Vector2D startPoint, Vector2D endPoint, double thickness)
        {
            a = startPoint.CopyToVector();
            b = endPoint.CopyToVector();
            this.thickness = thickness;
        }

        public void Draw(DrawingContext dc)
        {
            var pen = new Pen(Brushes.White, thickness);
            var p0 = new Point(a.X, a.Y);
            var p1 = new Point(b.X, b.Y);
            dc.DrawLine(pen, p0, p1);
        }
    }
}
