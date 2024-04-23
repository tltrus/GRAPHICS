using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace WpfApp
{
    // Water Ripples Based on:
    // Video: https://youtu.be/BZUdGqeOD0w
    // Algorithm: https://web.archive.org/web/20160418004149/http://freespace.virgin.net/hugo.elias/graphics/x_water.htm

    // Metaballs based on https://thecodingtrain.com/challenges/28-metaballs

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer RipplesTimer, MetaballsTimer;
        public static Random rnd = new Random();
        WriteableBitmap wb1, wb2;

        WaterRipples WaterRipples { get; set; }
        MetaBalls MetaBalls { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            // WaterRipples
            wb1 = new WriteableBitmap((int)g1.Width, (int)g1.Height, 96, 96, PixelFormats.Bgr32, null);
            g1.Source = wb1;
            WaterRipples = new WaterRipples((int)g1.Width, (int)g1.Height);

            // MetaBalls
            wb2 = new WriteableBitmap((int)g2.Width, (int)g2.Height, 96, 96, PixelFormats.Bgr32, null);
            g2.Source = wb2;
            MetaBalls = new MetaBalls((int)g2.Width, (int) g2.Height);

            RipplesTimer = new System.Windows.Threading.DispatcherTimer();
            RipplesTimer.Tick += new EventHandler(TimerRipplesTick);
            RipplesTimer.Interval = new TimeSpan(0, 0, 0, 0, 3);
            RipplesTimer.Start();


            MetaballsTimer = new System.Windows.Threading.DispatcherTimer();
            MetaballsTimer.Tick += new EventHandler(MetaballsTick);
            MetaballsTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            MetaballsTimer.Start();
        }

        private void TimerRipplesTick(object sender, EventArgs e) => WaterRipples.Drawing(wb1);
        private void MetaballsTick(object sender, EventArgs e) => MetaBalls.Drawing(wb2);


        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mousepos = e.GetPosition(g1);
            WaterRipples.Click((int)mousepos.X, (int)mousepos.Y);
        }
    }
}
