using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height, sceneW, sceneH;

        DrawingVisual visual1, visual2;
        DrawingContext dc1, dc2;
        List<Boundary> walls = new List<Boundary>();
        Particle particle;
        int xoff = 0;
        int yoff = 10000;
        GrowTree maze;

        public MainWindow()
        {
            InitializeComponent();

            visual1 = new DrawingVisual(); visual2 = new DrawingVisual();

            width = (int)g1.Width;
            height = (int)g1.Height;

            sceneW = (int)g2.Width;
            sceneH = (int)g2.Height;

            maze = new GrowTree(g1, 62); // Создание лабиринта
            walls = maze.GetWalls();

            // Стены по периметру
            walls.Add(new Boundary(0, 0, width, 0));
            walls.Add(new Boundary(0, height, width, height));
            walls.Add(new Boundary(0, 0, 0, height));
            walls.Add(new Boundary(width, 0, width, height));

            particle = new Particle(width / 2, height / 2);

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        { 
            DrawingG1();
            DrawingG2();
        }

        private void DrawingG1()
        {
            g1.RemoveVisual(visual1);
            using (dc1 = visual1.RenderOpen())
            {
                foreach(var wall in walls)
                    wall.Show(dc1);

                particle.Show(dc1);
                particle.Look(walls, dc1);

                dc1.Close();
                g1.AddVisual(visual1);
            }
        }

        private void DrawingG2()
        {
            g2.RemoveVisual(visual2);
            using (dc2 = visual2.RenderOpen())
            {
                double distProjPlane = sceneW / 2.0 / Math.Tan(particle.fov / 2.0); // projection plane is required for fisheye fix
                var scene = particle.Look(walls);
                double w = sceneW / scene.Count;

                for (int i = 0; i < scene.Count; i++)
                {
                    double d = scene[i] / sceneW;
                    double sq = scene[i] * scene[i];
                    double wSq = sceneW * sceneW;

                    var b = (byte)Map(sq, 0, wSq, 255, 0);
                    double h = 30 / d;

                    Rect rect = new Rect(i * w + w / 2, sceneH / 2 - h / 2, w + 1, h);
                    Brush brush = new SolidColorBrush(Color.FromRgb(b,b,b));
                    dc2.DrawRectangle(brush, null, rect);
                }

                dc2.Close();
                g2.AddVisual(visual2);
            }
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbSlider != null)
            {
                var fov = (int)slider.Value;
                lbSlider.Content = fov.ToString();

                particle.UpdateFOV(fov);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                particle.Move(2);
            } 
            else 
            if (e.Key == Key.S)
            {
                particle.Move(-2);
            } 
            else 
            if (e.Key == Key.A)
            {
                particle.Rotate(-0.1);
            } 
            else 
            if (e.Key == Key.D)
            {
                particle.Rotate(0.1);
            }
        }

        public static double Constrain(double n, double low, double high)
        {
            return Math.Max(Math.Min(n, high), low);
        }

        public static double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            double num = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return num;
            }

            if (start2 < stop2)
            {
                return Constrain(num, start2, stop2);
            }

            return Constrain(num, stop2, start2);
        }
    }
}
