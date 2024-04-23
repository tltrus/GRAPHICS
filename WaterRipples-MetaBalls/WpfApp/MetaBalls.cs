using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfApp
{
    internal class MetaBalls
    {
        int alpha;
        int width, height;
        static Random rnd = new Random();

        class Blob
        {
            public int r;
            int width, height;
            double xspeed, yspeed;
            public double x, y;

            public Blob(int width, int height)
            {
                this.width = width;
                this.height = height;
                var angle = rnd.NextDouble(0, 2 * Math.PI);
                xspeed = rnd.Next(2, 5) * Math.Cos(angle);
                yspeed = rnd.Next(2, 5) * Math.Sin(angle);
                r = rnd.Next(2, 15);
                x = rnd.Next(width);
                y = rnd.Next(height);
            }

            public void Update()
            {
                x += xspeed;
                y += yspeed;
                if (x > width || x < 0) xspeed *= -1;
                if (y > height || y < 0) yspeed *= -1;
            }
        }
        List<Blob> blobs = new List<Blob>();

        public MetaBalls(int width, int height) 
        {
            this.width = width;
            this.height = height;
            
            for (int i = 0; i < 3; i++)
                blobs.Add(new Blob(width, height));
        }

        public void Drawing(WriteableBitmap wb)
        {
            double sum = 0.0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sum = 0.0;
                    for (int i = 0; i < blobs.Count; i++)
                    {
                        var d = Dist(x, y, blobs[i].x, blobs[i].y);
                        sum += 1000 * blobs[i].r / d;
                    }

                    byte col = (byte)Tools.Map(sum, 0, 1000, 0, 255, true);
                    wb.SetPixel(x, y, Color.FromRgb(col, col, col));
                }
            }

            foreach (var b in blobs)
            {
                b.Update();
            }
        }

        private double Dist(double x1, double y1, double x2, double y2) => Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
    }
}
