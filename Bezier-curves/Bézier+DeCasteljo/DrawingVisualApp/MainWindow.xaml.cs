using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();
        int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        Point mouse;
        Bezier bezier;
        DeCasteljo deCasteljo;
        int drag_point_index = -1;
        List<Point> points;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            Init();

            Drawing();
        }


        void Init()
        {
            mouse = new Point();

            points = new List<Point>(){
                        new Point(150, 200),
                        new Point(200, 100),
                        new Point(500, 250),
                        new Point(400, 50)
                    };
            bezier = new Bezier(points);
            deCasteljo = new DeCasteljo(points);
        }

        void Drawing()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                if (cbBezier.IsChecked == true)
                    bezier.Draw(dc);

                if (cbDeCasteljo.IsChecked == true)
                    deCasteljo.Draw(dc);

                // Draw lines
                for (int i = 1; i < points.Count - 1; ++i)
                {
                    dc.DrawLine(new Pen(Brushes.White, 0.5), points[i - 1], points[i]);
                    dc.DrawLine(new Pen(Brushes.White, 0.5), points[i], points[i + 1]);
                }

                // Draw points
                foreach (Point p in points)
                {
                    dc.DrawEllipse(Brushes.White, null, p, 5, 5);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = e.GetPosition(g);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (drag_point_index < 0) return;

                bezier.points[drag_point_index] = mouse;
                
                Drawing();
            }
        }

        private int GetPointIndex(Point mouse)
        {
            foreach(Point p in bezier.points)
            {
                if (mouse.X > p.X - 5 && mouse.X < p.X + 5 && mouse.Y > p.Y - 5 && mouse.Y < p.Y + 5)
                {
                    return bezier.points.IndexOf(p);
                }
            }
            return -1; 
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_point_index = GetPointIndex(mouse);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drag_point_index = -1;
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Drawing();
        }
    }
}
