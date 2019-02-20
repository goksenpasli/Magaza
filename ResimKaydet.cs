namespace Magaza
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    public class ResimKaydet : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                var resim = parameter as byte[];
                if (resim == null) return;
                var dialog = new SaveFileDialog
                {
                    Filter = "Jpg Image|*.jpg",
                    FileName = "Resim"
                };
                if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, resim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}