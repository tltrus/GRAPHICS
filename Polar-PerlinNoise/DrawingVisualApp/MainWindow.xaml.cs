using System;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    // Based on "#136 — Polar Noise Loops" https://thecodingtrain.com/challenges/136-polar-noise-loops

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public Random rnd = new Random();
        public int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        double zoff, phase;
        int noiseMax = 10;


        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void slNoise_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => noiseMax = (int)slNoise.Value;

        private void Drawing()
        {
            PointCollection points = new PointCollection();
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                (double x, double y) = PointPositionCalculation(0, noiseMax);
                geometryContext.BeginFigure(new Point(x, y), true, true);


                for (double a = 0; a < Math.PI * 2; a += Radians(5))
                {
                    (x, y) = PointPositionCalculation(a, noiseMax);
                    points.Add(new Point(x, y));
                }

                geometryContext.PolyLineTo(points, true, true);
            }

            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                dc.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.White, 2), streamGeometry);
                
                dc.Close();
                g.AddVisual(visual);
            }

            phase += 0.003;
            zoff += 0.01;
        }

        private (double, double) PointPositionCalculation(double a, int noiseMax)
        {
            var xoff = Map(Math.Cos(a + phase), -1, 1, 0, noiseMax);
            var yoff = Map(Math.Sin(a + phase), -1, 1, 0, noiseMax);
            var r = Map(Perlin3D.perlin(xoff, yoff, zoff), 0, 1, 100, height / 2);
            var x = r * Math.Cos(a) + width / 2;
            var y = r * Math.Sin(a) + height / 2;

            return (x, y);
        }

        public double Constrain(double n, double low, double high) => Math.Max(Math.Min(n, high), low);
        public double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
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
        public double Radians(double degree) => degree * Math.PI / 180.0;
    }
}
