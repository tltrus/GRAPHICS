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

namespace AlgoritmBrezenhemWPF
{
    class CoordinateGrid
    {
        public Canvas gridPanel { get; set; }
        double width, height;

        public CoordinateGrid(Canvas g)
        {
            gridPanel = g;
            this.width = g.Width;
            this.height = g.Height;

            PaintAxis(gridPanel);
            PaintGrid();
            
        }

        // Отрисовка сетки
        private void PaintGrid()
        {
            int offs = 0;
            for (int i = 0; i < (int)gridPanel.Width / 10; i++)
            {
                Line lineV = new Line();
                lineV.X1 = 0;
                lineV.Y1 = lineV.Y2 = offs;
                lineV.X2 = gridPanel.Width;
                lineV.StrokeThickness = 1;
                lineV.Stroke = Brushes.LightGray;
                gridPanel.Children.Add(lineV);

                Line lineH = new Line();
                lineH.X1 = lineH.X2 = offs;
                lineH.Y1 = gridPanel.Height;
                lineH.Y2 = 0;
                lineH.StrokeThickness = 1;
                lineH.Stroke = Brushes.LightGray;
                gridPanel.Children.Add(lineH);

                offs += 10;
            }
        }

        // Отрисовка осей координат
        private void PaintAxis(Canvas p)
        {
            for (int i = -(int)p.Width / 2 / 10; i < (int)p.Width / 2 / 10; i++)
            {
                SetPixel(0, i, Brushes.Yellow, p, null);
                SetPixel(i, 0, Brushes.Yellow, p, null);
            }
        }

        // Формула отрисовки пикселя по входным координатам
        public static void SetPixel(int x, int y, SolidColorBrush strokeColor, Canvas p, SolidColorBrush fillColor)
        {
            x = ConvertXYaxis(x, true, (int)p.Width, (int)p.Height);
            y = ConvertXYaxis(y, false, (int)p.Width, (int)p.Height);

            Rectangle pixel = new Rectangle();
            Canvas.SetLeft(pixel, (double)x * 10);
            Canvas.SetTop(pixel, (double)y * 10);
            pixel.Width = 10;
            pixel.Height = 10;
            pixel.Fill = fillColor;
            pixel.StrokeThickness = 1;
            pixel.Stroke = strokeColor;
            p.Children.Add(pixel);
        }

        // Преобразование координат в нормальную систему координат
        private static int ConvertXYaxis(int val, bool axis, int width, int height)
        {
            if (axis)   // X
            {
                val = val + width / 20;
            }
            else        // Y
            {

                val = (-val - 1) + height / 20;
            }
            return val;
        }
    }
}
