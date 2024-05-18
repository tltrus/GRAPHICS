using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Diffuse
{
    // Based on Coding Challenge #13 — Reaction Diffusion Algorithm https://thecodingtrain.com/challenges/13-reaction-diffusion
    public partial class MainWindow : Window
    {
        class Cell
        {
            public double a { get; set; }
            public double b { get; set; }
        }

        Random rnd = new Random();
        System.Windows.Threading.DispatcherTimer timer1, timer2;
        WriteableBitmap wb;
        int width, height;

        double dA = 1;
        double dB = 0.5;
        double feed = 0.055;
        double k = 0.062;

        Cell[,] grid, next;

        public MainWindow()
        {
            InitializeComponent();
            
            width = (int)g.Width;
            height = (int)g.Height;
            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            g.Source = wb;

            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(timer1Tick);
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 5);

            timer2 = new System.Windows.Threading.DispatcherTimer();
            timer2.Tick += new EventHandler(timer2Tick);
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 20);

            grid = new Cell[height, width];
            next = new Cell[height, width];

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    grid[y, x] = new Cell() { a = 1, b = 0 };
                    next[y, x] = new Cell() { a = 1, b = 0 };
                }

            for (int i= height/2 - 10; i < height / 2; ++i)
                for (int j = width / 2 - 10; j < width / 2; ++j)
                    grid[i, j].b = 1;

            timer1.Start();
            timer2.Start();
        }

        private void timer1Tick(object sender, EventArgs e) => Control();
        private void timer2Tick(object sender, EventArgs e) => DrawFast();

        private void Control()
        {
            for (int y = 1; y < height - 1; y++)
                for (int x = 1; x < width - 1; x++)
                {
                    var a = grid[y, x].a;
                    var b = grid[y, x].b;

                    next[y, x].a = a + dA * LaplaceA(x, y) - a * b * b + feed * (1 - a);
                    next[y, x].b = b + dB * LaplaceB(x, y) + a * b * b - (k + feed) * b;

                    next[y, x].a = Constrain(next[y, x].a, 0, 1);
                    next[y, x].b = Constrain(next[y, x].b, 0, 1);
                }
            Swap();
        }

        // Slow method
        private void Draw()
        {
            // Присваивание цвета пикселю
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    var a = next[y, x].a;
                    var b = next[y, x].b;
                    var c = Math.Floor((a - b) * 255);
                    c = Constrain(c, 0, 255);

                    wb.SetPixel(x, y, (byte)c, (byte)c, (byte)c);
                }
        }
        private void DrawFast()
        {
            try
            {
                // Reserve the back buffer for updates.
                wb.Lock();

                // Присваивание цвета пикселю
                for (int y = 0; y < height; ++y)
                    for (int x = 0; x < width; ++x)
                    {
                        var a = next[y, x].a;
                        var b = next[y, x].b;
                        var c = Math.Floor((a - b) * 255);
                        byte color = (byte)Constrain(c, 0, 255);

                        unsafe
                        {
                            // Get a pointer to the back buffer.
                            IntPtr pBackBuffer = wb.BackBuffer;

                            // Find the address of the pixel to draw.
                            pBackBuffer += y * wb.BackBufferStride;
                            pBackBuffer += x * 4;

                            // {B, G, R, Apha}
                            int color = BitConverter.ToInt32(new byte[] { color, color, color, 255 }, 0);

                            // Assign the color data to the pixel.
                            *((int*)pBackBuffer) = color;
                        }

                        // Specify the area of the bitmap that changed.
                        wb.AddDirtyRect(new Int32Rect(x, y, 1, 1));
                    }
            }
            finally
            {
                // Release the back buffer and make it available for display.
                wb.Unlock();
            }
        }

        private double LaplaceA(int x, int y)
        {
            double sumA = 0;
            sumA += grid[y, x].a * -1;
            sumA += grid[y, x - 1].a * 0.2;
            sumA += grid[y, x + 1].a * 0.2;
            sumA += grid[y + 1, x].a * 0.2;
            sumA += grid[y - 1, x].a * 0.2;
            sumA += grid[y - 1, x - 1].a * 0.05;
            sumA += grid[y - 1, x + 1].a * 0.05;
            sumA += grid[y + 1, x + 1].a * 0.05;
            sumA += grid[y + 1, x - 1].a * 0.05;
            return sumA;
        }
        private double LaplaceB(int x, int y)
        {
            double sumB = 0;
            sumB += grid[y, x].b * -1;
            sumB += grid[y, x - 1].b * 0.2;
            sumB += grid[y, x + 1].b * 0.2;
            sumB += grid[y + 1, x].b * 0.2;
            sumB += grid[y - 1, x].b * 0.2;
            sumB += grid[y - 1, x - 1].b * 0.05;
            sumB += grid[y - 1, x + 1].b * 0.05;
            sumB += grid[y + 1, x + 1].b * 0.05;
            sumB += grid[y + 1, x - 1].b * 0.05;
            return sumB;
        }

        private void Swap()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    double temp_a = 0.0;
                    double temp_b = 0.0;

                    temp_a = grid[y, x].a;
                    temp_b = grid[y, x].b;

                    grid[y, x].a = next[y, x].a;
                    grid[y, x].b = next[y, x].b;

                    next[y, x].a = temp_a;
                    next[y, x].b = temp_b;
                }
        }

        private double Constrain(double n, double low, double high) => Math.Max(Math.Min(n, high), low);
    }
}
