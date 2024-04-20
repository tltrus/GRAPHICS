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
        public static double speed;
        public static int width, height, offsX, offsY;

        DrawingVisual visual;
        DrawingContext dc;
        List<Star> stars = new List<Star>();

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;
            offsX = width / 2;
            offsY = height / 2;

            for (int i = 0; i < 700; i++)
            {
                var x = rnd.Next(-width, width);
                var y = rnd.Next(-height, height);
                var z = rnd.Next(0, width);

                stars.Add(new Star(x, y, z));
            }

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) => Drawing();
        private void Drawing()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                foreach(var star in stars)
                {
                    star.Update();
                    star.Show(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.GetPosition(g).X;
            speed = Star.Map(x, 0, width, 0, 50);
        }
    }
}
