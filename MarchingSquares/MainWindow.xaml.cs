using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MarchingSquaresApp
{
    // Based on #C5 — Marching Squares - https://thecodingtrain.com/challenges/c5-marching-squares

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer1, timer2;
        static Random rnd = new Random();
        WriteableBitmap wb;
        int width, height;

        double[,] field;
        int rez = 10;
        int cols, rows;
        double increment = 0.1;
        double zoff;
        double noise;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            width = (int)image.Width; 
            height = (int)image.Height;

            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null); image.Source = wb;

            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(timer1Tick);
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 50);

            timer2 = new System.Windows.Threading.DispatcherTimer();
            timer2.Tick += new EventHandler(timer2Tick);
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 50);

            cols = 1 + width / rez;
            rows = 1 + height / rez;

            field = new double[rows, cols];

            timer1.Start();
            timer2.Start();
        }


        private void timer1Tick(object sender, EventArgs e)
        {
            double yoff = 0;
            for (int i = 0; i < rows; i++)
            {
                yoff += increment;
                double xoff = 0;
                for (int j = 0; j < cols; j++)
                {
                    noise = Perlin3D.perlin(xoff, yoff, zoff) * 1.2; // 3D
                    field[i, j] = Math.Round(noise);

                    xoff += increment;
                }
            }
            zoff += 0.03;
        }

        private void timer2Tick(object sender, EventArgs e)
        {
            wb.Clear(Colors.Black);
            Draw();
        }

        void Draw()
        {
            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < cols - 1; j++)
                {
                    int y = i * rez;
                    int x = j * rez;

                    var a = new Point(x + rez * 0.5, y);
                    var b = new Point(x + rez, y + rez * 0.5);
                    var c = new Point(x + rez * 0.5, y + rez);
                    var d = new Point(x, y + rez * 0.5);
                    int state = GetState(
                      (int)field[i, j],
                      (int)field[i, j + 1],
                      (int)field[i + 1, j + 1],
                      (int)field[i + 1, j]
                    );

                    switch (state)
                    {
                        case 1:
                            DrawLine(c, d);
                            break;
                        case 2:
                            DrawLine(b, c);
                            break;
                        case 3:
                            DrawLine(b, d);
                            break;
                        case 4:
                            DrawLine(a, b);
                            break;
                        case 5:
                            DrawLine(a, d);
                            DrawLine(b, c);
                            break;
                        case 6:
                            DrawLine(a, c);
                            break;
                        case 7:
                            DrawLine(a, d);
                            break;
                        case 8:
                            DrawLine(a, d);
                            break;
                        case 9:
                            DrawLine(a, c);
                            break;
                        case 10:
                            DrawLine(a, b);
                            DrawLine(c, d);
                            break;
                        case 11:
                            DrawLine(a, b);
                            break;
                        case 12:
                            DrawLine(b, d);
                            break;
                        case 13:
                            DrawLine(b, c);
                            break;
                        case 14:
                            DrawLine(c, d);
                            break;
                    }
                }
            }
        }

        int GetState(int a, int b, int c, int d) => a * 8 + b * 4 + c * 2 + d * 1;
        void DrawLine(Point p1, Point p2) => wb.DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, Colors.White);
    }
}
