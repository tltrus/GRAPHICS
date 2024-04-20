using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        private int width, height;

        DrawingVisual visual;
        DrawingContext dc;
        List<Drop> drops = new List<Drop>();

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            for (int i = 0; i < 500; i++)
            {
                drops.Add(new Drop(width, height));          // z
            }

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                foreach(var drop in drops)
                {
                    drop.Fall();
                    drop.Show(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
