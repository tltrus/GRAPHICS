using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    // Based on: Coding Challenge #103. Fire Effect https://thecodingtrain.com/challenges/103-fire-effect
    // Algorithm is not finished

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        WriteableBitmap wb;

        byte[,] buffer1, buffer2;
        int w, h;


        public MainWindow()
        {
            InitializeComponent();

            w = (int)g.Width;
            h = (int)g.Height;

            buffer1 = new byte[h, w];
            buffer2 = new byte[h, w];

            wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr32, null);
            g.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);


            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            for (int x = 0; x < w; ++x)
            {
                if (rnd.NextDouble() < 0.3)
                {
                    buffer1[h - 1, x] = 255;
                    buffer1[h - 2, x] = 255;
                }
                else
                {
                    buffer1[h - 1, x] = 0;
                    buffer1[h - 2, x] = 0;
                }
            }

            for (int y = 1; y < h - 1; ++y)
            {
                for (int x = 1; x < w - 1; ++x)
                {
                    var n1 = buffer1[y, x + 1];
                    var n2 = buffer1[y, x - 1];
                    var n3 = buffer1[y + 1, x];
                    var n4 = buffer1[y - 1, x];

                    var c = rnd.NextDouble();

                    var p = (n1 + n2 + n3 + n4) * 0.25;
                    p = p - c;

                    if (p < 0) p = 0;

                    buffer2[y - 1, x] = (byte)p;
                }
            }

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var color = buffer2[y, x];

                    wb.SetPixel(x, y, color, color, color);
                }
            }

            Swap(ref buffer1, ref buffer2);

        }

        void Swap<T>(ref T[,] matrixA, ref T[,] matrixB)
        {
            int length = matrixA.GetLength(0);
            int length2 = matrixA.GetLength(1);
            int length3 = matrixB.GetLength(0);
            int length4 = matrixB.GetLength(1);
            if (length2 != length4 || length != length3)
            {
                throw new Exception("Размерность матриц не одинакова");
            }

            T[,] array = Create<T>(length, length4);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    array[i, j] = matrixA[i, j];
                    matrixA[i, j] = matrixB[i, j];
                    matrixB[i, j] = array[i, j];
                }
            }
        }

        static T[,] Create<T>(int rows, int cols)
        {
            return new T[rows, cols];
        }
    }
}
