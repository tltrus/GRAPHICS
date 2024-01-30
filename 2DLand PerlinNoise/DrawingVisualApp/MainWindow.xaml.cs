using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MiscUtils.Numerics;

namespace DrawingVisualApp
{
    /// <summary>
    /// Based on
    /// https://github.com/BarneyWhiteman/CodingChallenges/blob/master/cc10_2d_terrain/terrain2D/terrain2D.pde
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();
        int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        int tileSize = 16;
        double scl = 7;
        double xRO, yRO, xTO, yTO, x, y;
        double speed = 16;
        int buffer = 0;
        int w, h;

        class BrushColor
        {
            public Brush grass;
            public Brush sand;
            public Brush forest;
            public Brush water;
        }
        BrushColor brushcolor;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            Init();
        }

        void Init()
        {
            w = width / tileSize + buffer;
            h = height / tileSize + buffer;

            brushcolor = new BrushColor()
            {
                grass = new SolidColorBrush(Color.FromRgb(34, 193, 14)),
                sand = new SolidColorBrush(Color.FromRgb(211, 182, 77)),
                forest = new SolidColorBrush(Color.FromRgb(30, 195, 10)),
                water = new SolidColorBrush(Color.FromRgb(48, 170, 255))
            };

            Drawing();
        }
        void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                Brush brush = null;
                xRO = x % tileSize;
                yRO = y % tileSize;
                xTO = (int)x / tileSize;
                yTO = (int)y / tileSize;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        int value = getTile(j, i);

                        Rect rect = new Rect()
                        {
                            Height = tileSize,
                            Width = tileSize,
                            X = (j - buffer / 2) * tileSize - xRO,
                            Y = (i - buffer / 2) * tileSize - yRO
                        };

                        switch (value)
                        {
                            case 0:
                                brush = brushcolor.water;
                                break;
                            case 1:
                                brush = brushcolor.sand;
                                break;
                            case 2:
                                brush = brushcolor.grass;
                                break;
                            case 3:
                                brush = brushcolor.forest;
                                break;
                        }
                        dc.DrawRectangle(brush, new Pen(brush, 1), rect);
                    }
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Numerics.Perlin2DSeed = rnd.Next(100);
            Drawing();
        }

        int getTile(int x, int y)
        {
            double v = Numerics.Noise((xTO + x) * scl, (yTO + y) * scl, 2, 0.3);
            v = Numerics.Normalize(v);
            if (v < 0.35)
            {
                //water
                return 0;
            }
            else if (v < 0.5)
            {
                //sand
                return 1;
            }
            else if (v < 0.7)
            {
                //grass
                return 2;
            }
            else
            {
                //forest
                return 3;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.W:
                    y -= speed;
                    break;
                case Key.S:
                    y += speed;
                    break;
                case Key.A:
                    x -= speed;
                    break;
                case Key.D:
                    x += speed;

                    break;
            }
            Drawing();
        }
    }
}
