using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DrawingVisualApp
{
    class Circle
    {
        public int x, y;
        public double r;
        public bool growing;

        public Circle(int x, int y)
        {
            this.x = x;
            this.y = y;
            growing = true;
            r = 1;
        }

        public void Grow()
        {
            if (growing)
                r += 1;
        }

        public void Show(WriteableBitmap wb)
        {
            Point point = new Point(x, y);
            wb.DrawEllipseCentered((int)point.X, (int)point.Y, (int)r, (int)r, Colors.White);
        }

        public bool Edges()
        {
            return (
              x + r >= MainWindow.width ||
              x - r <= 0 ||
              y + r >= MainWindow.height ||
              y - r <= 0
            );
        }
    }
}
