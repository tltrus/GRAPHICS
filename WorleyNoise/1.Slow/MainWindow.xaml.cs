using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        static Random rnd = new Random();
        WriteableBitmap wb;
        int width, height;
        int frameCount;

        List<Vector3D> points = new List<Vector3D>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            width = (int)image1.Width; height = (int)image1.Height;

            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null); image1.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            Setup();

            timer.Start();
        }

        private void Setup()
        {
            for (int i = 0; i < 20; i++)
            {
                points.Add(new Vector3D(Random(width), Random(height), Random(width)));
            }

            wb.Clear(Colors.Black);
        }


        private void timerTick(object sender, EventArgs e)
        {
            frameCount++;

            wb.Clear(Colors.Black);

            Variant_2D();

            for(int i = 0; i < points.Count; i++)
            {
                var x = points[i].X;
                var y = points[i].Y;

                x += rnd.Next(-1, 2);
                y += rnd.Next(-1, 2);


                points[i] = new Vector3D(x, y, 0);
            }
        }

        void Variant_2D()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double[] distances = new double[points.Count];
                    for (int i = 0; i < points.Count; i++)
                    {
                        var d = Dist(x, y, points[i].X, points[i].Y);
                        distances[i] = d;
                    }
                    double[] sorted = new double[distances.Length];
                    Array.Copy(distances, sorted, distances.Length);
                    Array.Sort(sorted);

                    var n = 0;
                    var col = (byte)Tools.Map(sorted[n], 0, width/2, 0, 255);
                    var r = col;
                    var g = col;
                    var b = col;

                    wb.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                }
            }
        }


        void Variant_3D()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double[] distances = new double[points.Count];
                    for (int i = 0; i < points.Count; i++)
                    {
                        var v = points[i];
                        var z = frameCount % width;
                        var d = Dist(x, y, z, v.X, v.Y, v.Z);
                        distances[i] = d;
                    }

                    double[] sorted = new double[distances.Length];
                    Array.Copy(distances, sorted, distances.Length);
                    Array.Sort(sorted);

                    var r = (byte)Tools.Map(sorted[0], 0, 150, 0, 255);
                    var g = (byte)Tools.Map(sorted[1], 0, 50, 255, 0);
                    var b = (byte)Tools.Map(sorted[2], 0, 200, 255, 0);

                    wb.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                }
            }
        }



        private int Random(int limit) => rnd.Next(limit);
        private double Dist(double x1, double y1, double z1, double x2, double y2, double z2) => Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));
        private double Dist(double x1, double y1, double x2, double y2) => Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

    }
}
