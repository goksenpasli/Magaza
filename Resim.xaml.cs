namespace Magaza
{
    using Microsoft.Win32;
    using System.Windows;
    using System.Windows.Controls;
    using WebEye.Controls.Wpf;

    public partial class Resim : Window
    {
        public Resim()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            comboBox1.ItemsSource = new WebCameraControl().GetVideoCaptureDevices();

            if (comboBox1.Items.Count > 0) comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void OnImageButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "Bitmap Resmi|*.bmp" };
            if (dialog.ShowDialog() == true) webCameraControl.GetCurrentImage().Save(dialog.FileName);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) => startButton.IsEnabled = e.AddedItems.Count > 0;

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            var cameraId = (WebCameraId)comboBox1.SelectedItem;
            webCameraControl.StartCapture(cameraId);
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e) => webCameraControl.StopCapture();
    }
}