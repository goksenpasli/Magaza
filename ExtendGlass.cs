namespace Magaza
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    internal static class ExtendGlass
    {
        public const int WM_DWMCOMPOSITIONCHANGED = 0x031E;

        public static void ExtendDwmGlass(Window window, Thickness thikness)
        {
            try
            {
                int isGlassEnabled = 0;
                DwmIsCompositionEnabled(ref isGlassEnabled);
                if (Environment.OSVersion.Version.Major > 5 && isGlassEnabled > 0)
                {
                    WindowInteropHelper helper = new WindowInteropHelper(window);
                    HwndSource mainWindowSrc = HwndSource.FromHwnd(helper.Handle);
                    mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

                    System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(mainWindowSrc.Handle);
                    float dpiX = desktop.DpiX / 96;
                    float dpiY = desktop.DpiY / 96;

                    MARGINS margins = new MARGINS
                    {
                        cxLeftWidth = (int)(thikness.Left * dpiX),
                        cxRightWidth = (int)(thikness.Right * dpiX),
                        cyBottomHeight = (int)(thikness.Bottom * dpiY),
                        cyTopHeight = (int)(thikness.Top * dpiY)
                    };

                    window.Background = Brushes.Transparent;

                    int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                }
                else
                {
                    window.Background = SystemColors.WindowBrush;
                }
            }
            catch (DllNotFoundException)
            {
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int
           DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(ref int en);

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;

            public int cxRightWidth;

            public int cyTopHeight;

            public int cyBottomHeight;
        }
    }
}