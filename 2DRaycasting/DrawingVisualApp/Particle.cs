using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D pos;
        public List<Ray> rays = new List<Ray>();
        private double heading;     // величина смещения угла поворота
        public int fov = 45;        // угол обзора, fisheye fix

        public Particle(double x, double y)
        {
            pos = new Vector2D(x / 2, y / 2);

            for (int a = (-fov / 2); a < (fov / 2); a++)
            {
                rays.Add(new Ray(pos, Radians(a)));
            }
        }

        public void UpdateFOV(int fov)
        {
            this.fov = fov;
            rays.Clear();
            for (int a = -this.fov / 2; a < this.fov / 2; a += 1)
            {
                rays.Add(new Ray(pos, Radians(a) + heading));
            }
        }

        public void Rotate(double angle)
        {
            heading += angle;

            int i = 0;
            for (int a = (-fov / 2); a < (fov / 2); a++)
            {
                rays[i].SetAngle(Radians(a) + heading);
                i++;
            }
        }

        public void Move(double amt)
        {
            var vel = Vector2D.FromAngle(heading);
            vel.SetMag(amt);
            pos.Add(vel);


            int i = 0;
            for (int a = (-fov / 2); a < (fov / 2); a++)
            {
                rays[i].pos = pos.CopyToVector();
                rays[i].SetAngle(Radians(a) + heading);
                i++;
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

                    if (pt is null) continue;

                    var d = Vector2D.Dist(pos, pt);
                    if (d < record)
                    {
                        record = d;
                        closest = pt;
                    }
                }

                if (closest is null) continue;

                var p0 = new Point(pos.x, pos.y);
                var p1 = new Point(closest.x, closest.y);
                dc.DrawLine(new Pen(Brushes.White, 1), p0, p1);
            }
        }

        public List<double> Look(List<Boundary> walls)
        {
            List<double> scene = new List<double>();
            for (int i = 0; i < rays.Count; i++)
            {
                var ray = rays[i];
                Vector2D closest = null;
                var record = double.MaxValue;
                foreach (var wall in walls)
                {
                    var pt = ray.Cast(wall);

                    if (pt is null) continue;

                    var d = Vector2D.Dist(pos, pt);
                    if (d < record)
                    {
                        record = d;
                        closest = pt;
                    }
                }

                if (closest is null) continue;

                var p0 = new Point(pos.x, pos.y);
                var p1 = new Point(closest.x, closest.y);
                scene.Add(record);
            }
            return scene;
        }

        public void Show(DrawingContext dc)
        {
            var p = new Point(pos.x, pos.y);
            dc.DrawEllipse(Brushes.White, null, p, 2, 2);
        }

        public double Radians(double degree) => degree * Math.PI / 180;
    }
}
