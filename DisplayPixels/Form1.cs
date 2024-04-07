using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Read_dispaly_pixels
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        Random rnd = new Random();
        Graphics graphic;
        int x, y;
        MouseHook mh;
        Color color;
        List<Ball> balls;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
            graphic = Graphics.FromHdc(hdc);

            int screenWidth = Screen.GetWorkingArea(this).Width;
            int screenHeight = Screen.GetWorkingArea(this).Height;

            mh = new MouseHook();
            mh.SetHook();
            mh.MouseMoveEvent += mh_MouseMoveEvent;
            mh.MouseClickEvent += mh_MouseClickEvent;

            timer.Interval = 50;
            timer.Tick += timer_Tick;

            // All balls
            balls = new List<Ball>();
            for (int i = 0; i < 40; i++)
            {
                int x = rnd.Next(screenWidth);
                int y = rnd.Next(screenHeight);
                int size = rnd.Next(10, 100);

                var r = (byte)rnd.Next(255);
                var g = (byte)rnd.Next(255);
                var b = (byte)rnd.Next(255);
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, r, g, b));

                int speed = rnd.Next(5, 30);

                Ball ball = new Ball(x, y, size, brush, speed, screenHeight);

                balls.Add(ball);
            }
        }

        private void mh_MouseClickEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                graphic = Graphics.FromHdc(hdc);

                var r = (byte)rnd.Next(255);
                var g = (byte)rnd.Next(255);
                var b = (byte)rnd.Next(255);
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, r, g, b));

                graphic.FillEllipse(brush, new Rectangle(x - 25, y - 25, 50, 50));
            }
        }

        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            x = e.Location.X;
            y = e.Location.Y;

            lbX.Text = x.ToString();
            lbY.Text = y.ToString();

            color = Win32Api.GetPixelColor(x, y);
            pictureBox1.BackColor = color;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => mh.UnHook();

        private void btnStart_Click(object sender, EventArgs e) => timer.Start();

        void timer_Tick(object sender, EventArgs e)
        {
            foreach(var ball in balls)
            {
                ball.Update();
                ball.Draw(graphic);
            }
        }
    }
}

sealed class Win32Api
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

    [DllImport("Shell32.dll")]
    public static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    static public System.Drawing.Color GetPixelColor(int x, int y)
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        uint pixel = GetPixel(hdc, x, y);
        ReleaseDC(IntPtr.Zero, hdc);
        Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                        (int)(pixel & 0x0000FF00) >> 8,
                        (int)(pixel & 0x00FF0000) >> 16);
        return color;

    }


// Mouse Hook
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        public POINT pt;
        public int hwnd;
        public int wHitTestCode;
        public int dwExtraInfo;
    }
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    //Install hook
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
    //Uninstall hook
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);
    //Call the next hook
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
}