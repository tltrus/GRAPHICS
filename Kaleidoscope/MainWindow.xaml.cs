using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    // Based on Coding Challenge #155 — Kaleidoscope Snowflake https://thecodingtrain.com/challenges/155-kaleidoscope-snowflake
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        WriteableBitmap wb;
        int width, height;

        int mouseX, pmouseX, mouseY, pmouseY;
        int offsX, offsY;
        int thickness = 3;
        Color color = Colors.LightGray;
        int xoff;

        public MainWindow()
        {
            InitializeComponent();

            width = (int)g.Width; height = (int)g.Height;

            // Для Битмапа
            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null); g.Source = wb;
            offsX = width / 2;
            offsY = height / 2;
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            pmouseX = mouseX;
            pmouseY = mouseY;

            mouseX = (int)e.GetPosition(g).X - width / 2;
            mouseY = (int)e.GetPosition(g).Y - height / 2;

            int mx = mouseX;
            int my = mouseY;
            int pmx = pmouseX;
            int pmy = pmouseY;


            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                var hu = (byte)Tools.Map(Math.Sin(xoff), -1, 1, 0, 255);
                xoff++;

                color = Color.FromArgb(150, hu, hu, hu);

                wb.DrawLineAa(mx + offsX, my + offsY, pmx + offsX, pmy + offsY, color, thickness);

                Vector2D mirr = new Vector2D(mx, my * -1);
                Vector2D pmirr = new Vector2D(pmx, pmy * -1);
                wb.DrawLineAa((int)mirr.x + offsX, (int)mirr.y + offsY, (int)pmirr.x + offsX, (int)pmirr.y + offsY, color, thickness);


                Vector2D mnew = new Vector2D(mx, my);
                Vector2D pmnew = new Vector2D(pmx, pmy);

                for (int i = 0; i < 5; ++i)
                {
                    mnew.Rotate(Math.PI / 3);
                    pmnew.Rotate(Math.PI / 3);

                    wb.DrawLineAa((int)mnew.x + offsX, (int)mnew.y + offsY, (int)pmnew.x + offsX, (int)pmnew.y + offsY, color, thickness);

                    mirr = new Vector2D(mnew.x, mnew.y * -1);
                    pmirr = new Vector2D(pmnew.x, pmnew.y * -1);
                    wb.DrawLineAa((int)mirr.x + offsX, (int)mirr.y + offsY, (int)pmirr.x + offsX, (int)pmirr.y + offsY, color, thickness);
                }
            }
        }
    }
}
