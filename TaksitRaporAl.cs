namespace Magaza
{
    using System;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand AylıkGrafikRaporAl => new AylıkGrafik();

        public ICommand GünlükGrafikRaporAl => new GünlükGrafik();

        public ICommand MüşteriRaporAl => new MüşteriRaporla();

        public ICommand TaksitRaporAl => new TaksitRaporla();

        public ICommand YıllıkGrafikRaporAl => new YıllıkGrafik();

        private class AylıkGrafik : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.aylıksatıştahsilatgrafik.frx");
        }

        private class GünlükGrafik : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.günlüksatıştahsilatgrafik.frx");
        }

        private class MüşteriRaporla : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.müşteriler.frx");
        }

        private class TaksitRaporla : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.taksitödemedurumları.frx");
        }

        private class YıllıkGrafik : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => new Raporla("Magaza.Rapor.yıllıksatıştahsilatgrafik.frx");
        }
    }
}