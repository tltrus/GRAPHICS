using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Read_dispaly_pixels
{
    // Mouse Hook from https://programmersought.com/article/52345236236/

    internal class MouseHook
    {
        private Point point;
        private Point Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;

                }
            }
        }
        private int hHook;
        public const int WH_MOUSE_LL = 14;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_LBUTTONDOWN = 0x0201;
        public Win32Api.HookProc hProc;

        public MouseHook()
        {
            this.Point = new Point();
        }
        public int SetHook()
        {
            hProc = new Win32Api.HookProc(MouseHookProc);
            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                switch ((int)wParam)
                {
                    case (int)WM_LBUTTONDOWN:
                        if (MouseClickEvent != null)
                        {
                            this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                            var e = new MouseEventArgs(MouseButtons.Left, 0, point.X, point.Y, 0);
                            MouseClickEvent(this, e);
                        }
                        break;
                    case (int)WM_RBUTTONDOWN:
                        if (MouseClickEvent != null)
                        {
                            this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                            var e = new MouseEventArgs(MouseButtons.Right, 0, point.X, point.Y, 0);
                            MouseClickEvent(this, e);
                        }
                        break;
                    case (int)WM_MOUSEMOVE:
                        if (MouseMoveEvent != null)
                        {
                            this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                            var e = new MouseEventArgs(MouseButtons.Left, 0, point.X, point.Y, 0);
                            MouseMoveEvent(this, e);
                        }
                        break;
                    default:
                        break;
                }
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
        //Delegate + event (encapsulate the hooked message as an event, which will be processed by the caller)
        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;

    }
}
