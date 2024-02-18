using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Interpolation
{

    public partial class MainWindow : Window
    {
        WriteableBitmap wb1, wb2, tmp;
        int g1width, g1height, g2width, g2height;
        double mult = 0;

        struct Pixels
        {
            public byte blue;
            public byte green;
            public byte red;
            public byte alpha;

            public Pixels(byte R, byte G, byte B)
            {
                blue = B;
                green = G;
                red = R;
                alpha = 255;
            }
            public Color ToColor()
            {
                return Color.FromArgb(alpha, red, green, blue);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            wb1 = BitmapFactory.FromContent("img.jpg");
            g1.Source = wb1;

            g2width = g1width = (int)wb1.Width;
            g2height = g1height = (int)wb1.Height;

            wb2 = new WriteableBitmap(g2width, g2height, 96, 96, PixelFormats.Bgra32, null);
        }

        private void btnResizeUp_Click(object sender, RoutedEventArgs e)
        {
            mult = 2;
            Resize();
        }
        private void btnResizeDown_Click(object sender, RoutedEventArgs e)
        {
            mult = 0.5;
            Resize();
        }

        private void Resize()
        {
            Pixels[,] colorset = Parcolorset(wb1);
            Pixels[,] interp = Interlpolation(colorset);
            wb2 = Setbmp(interp, interp.GetLength(0), interp.GetLength(1));
            g2.Source = wb2;
        }

        private Pixels[,] Parcolorset(WriteableBitmap bmp)
        {
            int h = bmp.PixelHeight; 
            int w = bmp.PixelWidth;
            Color[,] color = new Color[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    color[y, x] = bmp.GetPixel(x, y);
                }
            };

            Pixels[,] pixels = new Pixels[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    pixels[y, x].red = color[y, x].R;
                    pixels[y, x].green = color[y, x].G;
                    pixels[y, x].blue = color[y, x].B;
                }
            };
            return pixels;
        }

        private Pixels[,] Interlpolation(Pixels[,] f)
        {
            int h = f.GetLength(0); 
            int w = f.GetLength(1);

            int new_h = (int)(mult * h);
            int new_w = (int)(mult * w);

            Pixels[,] pixels = new Pixels[new_h, new_w];

            // Если увеличиваем картинку
            if (h < new_h)
            {
                for (int y = 0; y < h - 1; y++)
                    for (int x = 0; x < w - 1; x++)
                        pixels[(int)(mult * y), (int)(mult * x)] = f[y, x];

                for (int y = 0; y < h - 1; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        int ypos = (int)(mult * y);
                        int xpos = (int)(mult * x);

                        pixels[ypos + 1, xpos].red = (byte)((pixels[ypos, xpos].red + pixels[ypos + 2, xpos].red) / 2);
                        pixels[ypos + 1, xpos].green = (byte)((pixels[ypos, xpos].green + pixels[ypos + 2, xpos].green) / 2);
                        pixels[ypos + 1, xpos].blue = (byte)((pixels[ypos, xpos].green + pixels[ypos + 2, xpos].green) / 2);

                        pixels[ypos, xpos + 1].red = (byte)((pixels[ypos, xpos].red + pixels[ypos, xpos + 2].red) / 2);
                        pixels[ypos, xpos + 1].green = (byte)((pixels[ypos, xpos].green + pixels[ypos, xpos + 2].green) / 2);
                        pixels[ypos, xpos + 1].blue = (byte)((pixels[ypos, xpos].blue + pixels[ypos, xpos + 2].blue) / 2);

                        pixels[ypos + 1, xpos + 1].red = (byte)((pixels[ypos, xpos].red + pixels[ypos, xpos + 2].red + pixels[ypos, xpos].red + pixels[ypos + 2, xpos].red) / 4);
                        pixels[ypos + 1, xpos + 1].green = (byte)((pixels[ypos, xpos].green + pixels[ypos, xpos + 2].green + pixels[ypos, xpos].green + pixels[ypos + 2, xpos].green) / 4);
                        pixels[ypos + 1, xpos + 1].blue = (byte)((pixels[ypos, xpos].blue + pixels[ypos, xpos + 2].blue + pixels[ypos, xpos].blue + pixels[ypos + 2, xpos].blue) / 4);
                    }
                }
            } else
            // если уменьшаем картинку
            if (h > new_h)
            {
                for (int y = 0; y < new_h - 1; y++)
                    for (int x = 0; x < new_w - 1; x++)
                        pixels[y, x] = f[(int)(y / mult), (int)(x / mult)];
            }
           return pixels;
        }

        private WriteableBitmap Setbmp(Pixels[,] fin, int h, int w)
        {
            WriteableBitmap tbmp = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
            for (int y = 0; y < h - 1; y++)
                for (int x = 0; x < w - 1; x++)
                {
                    tbmp.SetPixel(x, y, Color.FromRgb(fin[y, x].red, fin[y, x].green, fin[y, x].blue));
                }
            return tbmp;
        }
    }
}
