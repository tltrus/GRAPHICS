using System;

namespace DrawingVisualApp
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

        public Vector2D Lerp(double x, double y, double amt)
        {
            Vector2D vector2D = new Vector2D();
            this.x += (x - this.x) * amt;
            this.y += (y - this.y) * amt;
            return vector2D;
        }

        public Vector2D Lerp(Vector2D v, double amt)
        {
            Vector2D vector2D = new Vector2D();
            x += (v.x - x) * amt;
            y += (v.y - y) * amt;
            return vector2D;
        }

        public static Vector2D Lerp(Vector2D v1, Vector2D v2, double amt)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Lerp(v2, amt);
            return vector2D;
        }
    }

}
