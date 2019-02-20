namespace Magaza
{
    using System;
    using System.Windows.Input;

    public partial class Tahsilatlar
    {
        private class UpDownDeğişti : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                (Form.GbTahsilatlar.DataContext as Tahsilatlar).Kalan = Form.DuDToplamTutar.Value - Form.DudPeşinÖdenen.Value;
                if (Form.CbÖdemeTipi.SelectedIndex == 0)
                {
                    Form.DudPeşinÖdenen.Value = Form.DuDToplamTutar.Value;
                }
            }
        }
    }
}