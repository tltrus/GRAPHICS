using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace DrawingVisualApp
{

    // Based on #50 — Circle Packing https://thecodingtrain.com/challenges/50-animated-circle-packing
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw;
        public static Random rnd = new Random();
        public static int width, height;
        WriteableBitmap wb;

        List<Circle> circles = new List<Circle>();
        List<Point> draw_area = new List<Point>();


        public MainWindow()
        {
            InitializeComponent();


            width = (int)g.Width;
            height = (int)g.Height;

            wb = BitmapFactory.FromResource("Pictures/2024.png");
            g.Source = wb;

            timerDraw = new System.Windows.Threading.DispatcherTimer();
            timerDraw.Tick += new EventHandler(timerDrawTick);
            timerDraw.Interval = new TimeSpan(0, 0, 0, 0, 50);

            Setup();

            wb.Clear(Colors.Black);

            timerDraw.Start();
        }

        private void Setup()
        {
            for(int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    var color = wb.GetPixel(x, y);
                    if (color == Colors.White)
                    {
                        draw_area.Add(new Point(x, y));
                    }
                }
        }

        private void timerDrawTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            wb.Clear(Colors.Black);

            // Вывод по 5 штук
            var total = 5;
            var count = 0;
            var attempts = 0;
            while (count < total)
            {
                var newC = NewCircle();
                if (newC != null)
                {
                    circles.Add(newC);
                    count++;
                }
                attempts++;
                if (attempts > 100)
                {
                    timerDraw.Stop();
                    MessageBox.Show("Finished");
                    break;
                }
            }

            foreach (var c in circles)
            {
                if (c.growing)
                {
                    if (c.Edges())
                    {
                        c.growing = false;
                    }
                    else
                    {
                        foreach (var other in circles)
                        {
                            if (c != other)
                            {
                                var d = Dist(c.x, c.y, other.x, other.y);
                                if (d - 2 < c.r + other.r)
                                {
                                    c.growing = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                c.Show(wb);
                c.Grow();
            }
        }

        private Circle NewCircle()
        {
            var r = rnd.Next(draw_area.Count);
            var spot = draw_area[r];
            var x = (int)spot.X;
            var y = (int)spot.Y;

            var valid = true;
            for (var i = 0; i < circles.Count; i++)
            {
                var circle = circles[i];
                var d = Dist(x, y, circle.x, circle.y);
                if (d < circle.r)
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                return new Circle(x, y);
            }
            else
            {
                return null;
            }
        }

        public double Dist(int x1, int y1, int x2, int y2) => Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
}
