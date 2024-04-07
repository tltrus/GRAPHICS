using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    // Based on : "Ray Casting 2D. Coding Challenge #145" https://codingtrain.github.io/website-archive/CodingChallenges/145-2d-ray-casting.html

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public Random rnd = new Random();
        public int width, height;

        DrawingVisual visual;
        DrawingContext dc;
        List<Boundary> walls = new List<Boundary>();
        Particle particle;
        Vector2D mouse = new Vector2D();

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            for (int i = 0; i < 5; i++)
            {
                var x1 = rnd.Next(width);
                var y1 = rnd.Next(height);
                var x2 = rnd.Next(width);
                var y2 = rnd.Next(height);
                walls.Add(new Boundary(x1, y1, x2, y2));
            }
            // Walls around field
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

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach(var wall in walls)
                {
                    wall.Show(dc);
                }
                particle.Update(mouse);
                particle.Show(dc);
                particle.Look(walls, dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.X = e.GetPosition(g).X;
            mouse.Y = e.GetPosition(g).Y;
        }
    }
}
