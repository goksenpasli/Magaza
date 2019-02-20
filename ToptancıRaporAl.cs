namespace Magaza
{
    using System;
    using System.Windows.Input;

    public partial class Toptancilar
    {
        private class ToptancıRaporAl : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.toptancılar.frx");
        }
    }
}