using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Drop
    {
        double x, y, z, yspeed, len;
        int width, height, minSpeed, maxSpeed;

        public Drop(int width, int height)
        {
            this.width = width;
            this.height = height;

            minSpeed = 2;
            maxSpeed = 8;
            
            x = MainWindow.rnd.Next(0, width);
            y = MainWindow.rnd.Next(-500, -50);
            z = MainWindow.rnd.Next(0, 20);
            yspeed = Tools.Map(z, 0, 20, minSpeed, maxSpeed);
            len = Tools.Map(z, 0, 20, 15, 35);
        }

        public void Fall()
        {
            y += yspeed;

            if (y > height)
            {
                y = MainWindow.rnd.Next(-300, 0);
                yspeed = Tools.Map(z, 0, 20, minSpeed, maxSpeed);
            }
        }

        public void Show(DrawingContext dc)
        {
            var thick = Tools.Map(z, 0, 20, 1, 3);

            Rect rect = new Rect()
            {
                X = x,
                Y = y,
                Height = len,
                Width = thick
            };
            dc.DrawRoundedRectangle(null, new Pen(Brushes.LightGray, thick), rect, 1, 1);
        }
    }
}
