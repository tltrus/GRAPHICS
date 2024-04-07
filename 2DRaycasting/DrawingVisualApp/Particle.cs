using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D pos;
        public List<Ray> rays = new List<Ray>();

        public Particle(double x, double y)
        {
            pos = new Vector2D(x, y);

            for (int a = 0; a < 360; a += 1)
            {
                rays.Add(new Ray(pos, Numerics.ToRadians(a)));
            }
        }

        public void Update(Vector2D newpos)
        {
            pos.Set(newpos.X, newpos.Y);

            rays.Clear();
            for (int a = 0; a < 360; a += 1)
            {
                rays.Add(new Ray(pos, Numerics.ToRadians(a)));
            }
        }

        public void Look(List<Boundary> walls, DrawingContext dc)
        {
            for (int i = 0; i < rays.Count; i++)
            {
                var ray = rays[i];
                Vector2D closest = null;
                var record = double.MaxValue;
                foreach (var wall in walls)
                {
                    var pt = ray.Cast(wall);
                    if (pt is null)
                    { 
                         
                    } 
                    else
                    {
                        var d = Vector2D.Dist(pos, pt);
                        if (d < record)
                        {
                            record = d;
                            closest = pt;
                        }
                    }
                }
                if (closest is null)
                {

                } 
                else
                {
                    var p0 = new Point(pos.X, pos.Y);
                    var p1 = new Point(closest.X, closest.Y);
                    dc.DrawLine(new Pen(Brushes.White, 1), p0, p1);
                }
            }
        }

        public void Show(DrawingContext dc)
        {
            // ЧАСТИЦА
            var p = new Point(pos.X, pos.Y);
            dc.DrawEllipse(Brushes.White, null, p, 2, 2);

            // ЛУЧИ
            //foreach (var ray in rays)
            //{
            //    ray.Show(dc);
            //}
        }
    }
}
