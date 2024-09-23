using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MiscUtils.Numerics;

namespace WpfApp
{
    // Based on Codding Challenge #103. Fire Effect https://thecodingtrain.com/CodingChallenges/103-fire-effect.html

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer wb1_timer, wb2_timer;
        public static Random rnd = new Random();
        WriteableBitmap wb, wb2;

        byte[,] buffer1, buffer2, cooling;
        int w, h;
        double ystart = 0.0, xstart = 0.0;

        byte[,] map1, map2;
        int rows, cols;
        int cWidth = 5, cHeight = 5;


        public MainWindow()
        {
            InitializeComponent();

            w = (int)g.Width;
            h = (int)g.Height;

            cols = w / cWidth;
            rows = h / cHeight;

            buffer1 = new byte[rows, cols];
            buffer2 = new byte[rows, cols];
            cooling = new byte[rows, cols];

            wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr32, null);
            g.Source = wb;

            wb2 = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr32, null);
            g2.Source = wb2;

            map1 = new byte[rows, cols];
            map2 = new byte[rows, cols];

            wb1_timer = new System.Windows.Threading.DispatcherTimer();
            wb1_timer.Tick += new EventHandler(wb1_timerTick);
            wb1_timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            wb2_timer = new System.Windows.Threading.DispatcherTimer();
            wb2_timer.Tick += new EventHandler(wb2_timerTick);
            wb2_timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            wb1_timer.Start();
            wb2_timer.Start();
        }

        private void wb1_timerTick(object sender, EventArgs e)
        {
            Fire(2);
            Cool();

            for (int y = 1; y < rows - 1; y++)
            {
                for (int x = 1; x < cols - 1; x++)
                {
                    var n1 = buffer1[y, x + 1];
                    var n2 = buffer1[y, x - 1];
                    var n3 = buffer1[y + 1, x];
                    var n4 = buffer1[y - 1, x];

                    var c = cooling[y, x];

                    var p = (n1 + n2 + n3 + n4) * 0.25;
                    p = p - c;

                    if (p < 0) p = 0;

                    buffer2[y - 1, x] = (byte)p;
                }
            }

            DrawMap(wb, buffer2);

            // Swap buffers
            Numerics.Swap(ref buffer1, ref buffer2);
        }

        private void wb2_timerTick(object sender, EventArgs e)
        {
            if (cbCooling.IsChecked == true)
                DrawMap(wb2, map2);
            else
                wb2.Clear(Colors.Black);
        }

        private void Cool()
        {
            double xoff = 0; // Start yoff at 0
            double increment = 6;

            // For every x,y coordinate in a 2D space, calculate a noise value and produce a brightness value
            for (int x = 0; x < cols; x++)
            {
                xoff += increment; // Increment yoff

                double yoff = ystart; // For every xoff, start yoff at 0
                for (int y = 0; y < rows; y++)
                {
                    yoff += increment; // Increment xoff

                    // Calculate noise and scale by 255
                    double n = Numerics.Noise(xoff, yoff, 5, 0.5); //Numerics.Noise(xoff, yoff, 5, 0.9);
                    var bright = (int)(n * 255 + 80) & 255;

                    // Set each pixel onscreen to a grayscale value
                    cooling[y, x] = (byte)(bright - 255);
                    map2[y, x] = cooling[y, x];
                }
            }

            ystart += increment;
        }

        private void Fire(int rows_start)
        {
            for (int j = 0; j < rows_start; j++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int y = rows - (j + 1);

                    buffer1[y, x] = 255;
                }
            }
        }

        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void DrawMap(WriteableBitmap wb, byte[,] array)
        {
            var h = array.GetLength(0);
            var w = array.GetLength(1);
            
            for(int i = 0; i < h; ++i)
            {
                for(int j = 0; j < w; ++j)
                {
                    var c = array[i, j];
                    var color = Color.FromArgb(c,c,c,c);

                    var y1 = i * cHeight;
                    var x1 = j * cWidth;

                    var x2 = x1 + cWidth;
                    var y2 = y1 + cHeight;

                    wb.FillRectangle(x1,y1,x2,y2, color);
                }
            }
        }
    }
}
