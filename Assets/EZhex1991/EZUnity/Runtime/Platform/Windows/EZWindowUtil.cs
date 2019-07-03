/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-22 10:10:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace EZhex1991.EZUnity
{
    public static class EZWindowUtil
    {
        public struct Margin
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);
        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        [DllImport("user32.dll")]
        public static extern int SetLayeredWindowAttributes(IntPtr hWnd, int crKey, int bAlpha, int dwFlags);

        [DllImport("Dwmapi.dll")]
        public static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margin margins);

        public const int WS_POPUP = 0X800000;
        public const int WS_BOARDER = 0x00800000;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x20;

        public const int GWL_EXSTYLE = -20;
        public const int GWL_STYLE = -16;

        public const int SWP_SHOWWINDOW = 0x0040;

        public const int LWA_COLORKEY = 0x00000001;
        public const int LWA_ALPHA = 0x00000002;

        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_OPAQUE = 0x00000004;
        public const int ULW_EX_NORESIZE = 0x00000008;

        public static void RemoveFrameAndIcon(IntPtr hWnd)
        {
#if !UNITY_EDITOR
            long oldLong = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, oldLong & WS_POPUP);
#endif
        }
        public static void ShowFrame(IntPtr hWnd)
        {
#if !UNITY_EDITOR
            long oldLong = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, oldLong | WS_BOARDER | WS_CAPTION);
#endif
        }
        public static void HideFrame(IntPtr hWnd)
        {
#if !UNITY_EDITOR
            long oldLong = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, oldLong & ~WS_BOARDER & ~WS_CAPTION);
#endif
        }
        public static void SetTransparency(IntPtr hWnd, int colorKey)
        {
#if !UNITY_EDITOR
            Margin margin = new Margin { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hWnd, ref margin);
#endif
            SetLayeredWindowAttributes(hWnd, 0, 100, LWA_ALPHA);
        }
        public static void SetWindowPenetrate(IntPtr hWnd)
        {
#if !UNITY_EDITOR
            long oldLong = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, oldLong | WS_EX_TRANSPARENT | WS_EX_LAYERED);
#endif
        }
        public static void SetWindowPos(IntPtr hWnd, int x, int y, int width, int height)
        {
#if !UNITY_EDITOR
            SetWindowPos(hWnd, -1, x, y, width, height, SWP_SHOWWINDOW);
#endif
        }
        public static void SetLayeredAlpha(IntPtr hWnd, int alpha)
        {
#if !UNITY_EDITOR
            SetLayeredWindowAttributes(hWnd, 0, alpha, LWA_ALPHA);
#endif
        }
        public static void SetLayeredAlphaColor(IntPtr hWnd, int colorKey)
        {
#if !UNITY_EDITOR
            SetLayeredWindowAttributes(hWnd, colorKey, 0, LWA_COLORKEY);
#endif
        }
    }
}
#endif
