using System;
using System.Windows;
using System.Windows.Media;


namespace AlgoritmBrezenhemWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CoordinateGrid g1, g2, g3;

        public MainWindow()
        {
            InitializeComponent();

            g1 = new CoordinateGrid(panel1);
            g2 = new CoordinateGrid(panel2);
            g3 = new CoordinateGrid(panel3);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            // Алгоритм DDA.
            DDA_Brezenhem_Line(0, 0, 3, -7);
            DDA_Brezenhem_Line(0, 0, 3, 7);
            DDA_Brezenhem_Line(0, 0, -3, 7);
            DDA_Brezenhem_Line(0, 0, -3, -7);

            // Brezenhem 1
            BrezenhemLine_1(0, 0, 7, 3);

            // Brezenhem 2
            BrezenhemLine_2(0, 0, 3, -7);
            BrezenhemLine_2(0, 0, 7, 3);
            BrezenhemLine_2(0, 0, -3, 7);
            BrezenhemLine_2(0, 0, -3, -7);

        }

        // Алгоритм DDA.
        // Based on: https://compgraphics.info/2D/Brezenham.php
        private void DDA_Brezenhem_Line(int x1, int y1, int x2, int y2)
        {
            int dx = (x2 - x1 >= 0 ? 1 : -1);
            int dy = (y2 - y1 >= 0 ? 1 : -1);

            int lenX = Math.Abs(x2 - x1);
            int lenY = Math.Abs(y2 - y1);

            // Проверка что вторая точка равняется первой.
            // Если да, то закрашиваем точку и работа прекращается
            int length = Math.Max(lenX, lenY);
            if (length == 0) CoordinateGrid.SetPixel(x1, y1, Brushes.Red, panel1, Brushes.Red);

            if (lenX >= lenY)
            {
                int x = x1;
                double y = y1;

                // Основной цикл
                length++;
                while (length > 0)
                {
                    length--;
                    CoordinateGrid.SetPixel(x, (int)(y + 0.5*dy), Brushes.Red, panel1, Brushes.Red);
                    x += dx;
                    y += dy * (double)lenY / lenX;
                }

            } else
            {
                double x = x1;
                int y = y1;

                // Основной цикл
                length++;
                while (length > 0)
                {
                    length--;
                    CoordinateGrid.SetPixel((int)(x + 0.5*dx), y, Brushes.Red, panel1, Brushes.Red);
                    x += dx * (double)lenX / lenY;
                    y += dy;
                }
            }
  
        }

        // Алгоритм Брезенхема 1 (метод центральной точки)
        private void BrezenhemLine_1(int x1, int y1, int x2, int y2)
        {
            int x, y, dx, dy, count;
            double f;
            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            x = x1; y = y1;
            CoordinateGrid.SetPixel(x, y, Brushes.Red, panel2, Brushes.Red);
            count = dx;
            while (count > 0)
            {
                count = count - 1;
                f = dy * (x+1) - dx * (y+0.5) - (x1 * dy - y1 * dx);
                if (f > 0) y = y + 1;
                x = x + 1;
                CoordinateGrid.SetPixel(x, y, Brushes.Red, panel2, Brushes.Red);

            }
        }

        // Алгоритм Брезенхема 2 (метод центральной точки) Оптимизированный
        private void BrezenhemLine_2(int x1, int y1, int x2, int y2)
        {
            int x, y, dx, dy, incrE, incrNE, eFx, eFy, count;

            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            eFx = 2 * dy - dx; // для случая если X > Y
            eFy = 2 * dx - dy; // для случая если X < Y

            int qx = (x2 - x1 >= 0 ? 1 : -1);
            int qy = (y2 - y1 >= 0 ? 1 : -1);

            if (dx > dy)
            {
                x = x1; y = y1;
                CoordinateGrid.SetPixel(x, y, Brushes.Red, panel3, Brushes.Red);
                count = dx;
                while (count > 0)
                {
                    count = count - 1;
                    if (eFx > 0)
                    {
                        y = y + qy;
                        eFx = eFx + 2 * dy - 2 * dx;
                    }
                    else
                        eFx = eFx + 2 * dy;
                    x = x + qx;
                    CoordinateGrid.SetPixel(x, y, Brushes.Red, panel3, Brushes.Red);
                }
            } else
            {
                x = x1; y = y1;
                CoordinateGrid.SetPixel(x, y, Brushes.Red, panel3, Brushes.Red);
                count = dy;
                while (count > 0)
                {
                    count = count - 1;
                    if (eFy > 0)
                    {
                        x = x + qx;
                        eFy = eFy + 2 * dx - 2 * dy;
                    }
                    else
                        eFy = eFy + 2 * dx;
                    y = y + qy;
                    CoordinateGrid.SetPixel(x, y, Brushes.Red, panel3, Brushes.Red);
                }
            }


        }

    }
}
