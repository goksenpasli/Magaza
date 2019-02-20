namespace Magaza
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ExtendGlass.ExtendDwmGlass(this, new Thickness(-1));
            WindowExtensions.SystemMenu(this);
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(new HwndSourceHook(WindowExtensions.GlassWndProc));
        }
    }
}