namespace DrawingVisualApp
{
    class Ray
    {
        public Vector2D pos;
        public Vector2D dir; // direction

        public Ray(Vector2D pos, double angle)
        {
            this.pos = pos.CopyToVector();
            dir = Vector2D.FromAngle(angle);
            dir.Add(pos);
        }

        public void SetAngle(double angle)
        {
            dir = Vector2D.FromAngle(angle);
            dir.Add(pos);
        }

        public void LookAt(double x, double y)
        {
            dir.x = x - pos.x;
            dir.y = y - pos.y;
            dir.Normalize();
            dir.Add(pos);
        }

        public Vector2D Cast(Boundary wall)
        {
            var x1 = wall.a.x;
            var y1 = wall.a.y;
            var x2 = wall.b.x;
            var y2 = wall.b.y;

            var x3 = pos.x;
            var y3 = pos.y;
            var x4 = dir.x;
            var y4 = dir.y;

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
                pt.x = x1 + t * (x2 - x1);
                pt.y = y1 + t * (y2 - y1);
                return pt;
            }
            else
            {
                return null;
            }
        }
    }
}
