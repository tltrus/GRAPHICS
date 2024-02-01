using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PerlinNoise
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        WriteableBitmap wb;
        int imgWidth, imgHeight;    // Размеры в пикселях
        int cellWidth, cellHeight;  // Количество ячеек
        int cellSize; // Размер ячейки
        Perlin2D perlin2D;
        double[,] cells;

        int Seed;
        int Octaves;
        double Persistance;

        public MainWindow()
        {
            InitializeComponent();
            imgWidth = (int)image.Width; imgHeight = (int)image.Height;
            wb = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            image.Source = wb;
        }

        void PerlinControl ()
        {
            perlin2D = new Perlin2D(Seed);

            for (int y = 0; y < cellHeight; y++)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    double value = perlin2D.Noise(x * 0.01, y * 0.01, Octaves, Persistance);
                    //double value = Perlin3D.OctavePerlin(x, y, 0.1, Octaves, Persistance);

                    cells[y, x] = (int)(value * 255 + 128) & 255;

                    Color color = Color.FromRgb((byte)cells[y, x], (byte)cells[y, x], (byte)cells[y, x]);
                    wb.FillRectangle((x * cellSize), (y * cellSize), (x * cellSize) + cellSize, (y * cellSize) + cellSize, color);
                }
            }
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            cellSize = int.Parse(tbSize.Text);
            cellWidth = imgWidth / cellSize;
            cellHeight = imgHeight / cellSize;

            cells = new double[imgHeight, imgWidth];

            wb.Clear(Colors.White);

            Seed = int.Parse(tbSeed.Text);
            Octaves = int.Parse(tbOct.Text);
            Persistance = double.Parse(tbPers.Text);

            PerlinControl();
        }
    }
}
