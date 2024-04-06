using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MiscUtils;
using MiscUtils.Vectors;


namespace DrawingVisualApp
{
    /// <summary>
    /// Based on "Lightning Bolts" https://drilian.com/2009/02/25/lightning-bolts/
    /// https://habr.com/ru/articles/230483/
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;
        Vector2D mouse = new Vector2D();
        Vector2D temp = new Vector2D();
        Vector2D startPoint = new Vector2D();
        Vector2D endPoint = new Vector2D();
        List<Segment> segmentList;
        double offsetAmount, maximumOffset;
        double lengthScale;
        double thickness;


        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            Setup();

            timer.Start();
            Update();
            Drawing();
        }

        private void timerTick(object sender, EventArgs e)
        {
            Setup();
            Update();
            Drawing();
        }

        private void Setup()
        {
            segmentList = new List<Segment>();
            maximumOffset = 30;
            thickness = 4;
            lengthScale = 0.7;

            startPoint = new Vector2D(width/2, 20);
            endPoint = mouse.CopyToVector();
            segmentList.Add(new Segment(startPoint, endPoint, thickness));

            offsetAmount = maximumOffset;     // максимальное смещение вершины молнии
        }

        private void Update()
        {
            for (int i = 0; i < 6; ++i)
            {
                foreach (var s in segmentList.ToList())
                {
                    startPoint = s.a;
                    endPoint = s.b;
                    Vector2D midPoint = Average(startPoint, endPoint);

                    temp = Vector2D.Sub(endPoint, midPoint);
                    temp.Normalize();
                    temp.Rotate(-Math.PI / 2);
                    temp.Mult(rnd.NextDoubleRange(-offsetAmount, offsetAmount));
                    midPoint.Add(temp);

                    segmentList.RemoveAt(segmentList.IndexOf(s));

                    // Делаем два новых сегмента, из начальной точки к конечной
                    // и через новую (случайную) центральную
                    segmentList.Add(new Segment(startPoint, midPoint, thickness));
                    segmentList.Add(new Segment(midPoint, endPoint, thickness));

                    if (rnd.NextDouble() < 0.25)
                    {
                        // Создаем ответвления
                        Vector2D direction = Vector2D.Sub(midPoint, startPoint);
                        var randomSmallAngle = rnd.NextDoubleRange(-0.2, 0.2);
                        direction.Rotate(randomSmallAngle);
                        Vector2D splitEnd = direction.Mult(lengthScale) + midPoint; // lengthScale лучше взять < 1. С 0.7 выглядит неплохо.
                        segmentList.Add(new Segment(midPoint, splitEnd, thickness));
                        thickness -= 0.15;
                    }
                }
                
                offsetAmount /= 2; // Каждый раз уменьшаем в два раза смещение центральной точки по сравнению с предыдущей итерацией
            }
        }

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach(var s in segmentList)
                {
                    s.Draw(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private Vector2D Average(Vector2D startpoint, Vector2D endPoint)
        {
            var x = endPoint.X - startpoint.X;
            var y = endPoint.Y - startpoint.Y;

            return new Vector2D(startpoint.X + x / 2, startpoint.Y + y / 2);
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.X = e.GetPosition(g).X;
            mouse.Y = e.GetPosition(g).Y;
        }
    }
}
