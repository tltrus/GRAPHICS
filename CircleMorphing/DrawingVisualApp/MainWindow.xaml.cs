using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    /// <summary>
    /// Based on Circle Morphing https://thecodingtrain.com/challenges/81-circle-morphing
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        List<Vector2D> cirPath = new List<Vector2D>();
        List<Vector2D> triPath = new List<Vector2D>();
        double spacing = 10;
        double theta = 0;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            double radius = 100;
            double startA = 0;
            double endA = 120;
            Vector2D start = PolarToCartesian(radius, startA);
            Vector2D end = PolarToCartesian(radius, endA);
            for (var a = startA; a < 360; a += spacing)
            {
                Vector2D cv = PolarToCartesian(radius, a);
                cirPath.Add(cv);
                double amt = (a % 120) / (endA - startA);
                Vector2D tv = Vector2D.Lerp(start, end, amt);
                triPath.Add(tv);

                if ((a + spacing) % 120 == 0)
                {
                    startA = startA + 120;
                    endA = endA + 120;
                    start = PolarToCartesian(radius, startA);
                    end = PolarToCartesian(radius, endA);
                }
            }

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            List<Point> points = new List<Point>();

            var amt = (Math.Sin(theta) + 1) / 2;
            theta += 5;

            for (var i = 0; i < cirPath.Count; i++)
            {
                var cv = cirPath[i];
                var tv = triPath[i];
                var x = Tools.Lerp(cv.x, tv.x, amt);
                var y = Tools.Lerp(cv.y, tv.y, amt);
                points.Add(new Point(x + width * 0.5, y + height * 0.5));
            }

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], true, true);
                geometryContext.PolyLineTo(points, true, true);
            }

            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                dc.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.White, 2), streamGeometry);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private Vector2D PolarToCartesian(double r, double angle)
        {
            angle = Tools.ToRadians(angle);
            return new Vector2D(r * Math.Cos(angle), r * Math.Sin(angle));
        }
    }
}
