namespace Magaza
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand TaksitGör => new TaksitDurumları();
    }

    public partial class TaksitDurumları : ICommand
    {
        public static TaksitDurumu taksitDurumu;

        public ICommand ComboTaksitKişiDeğişti => new TaksitKişiDeğiştirme();

        public ObservableCollection<Taksitler> Cv { get; set; }

        public ICommand TaksitRaporAl => new TaksitKişiRaporla();

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                taksitDurumu = new TaksitDurumu { Owner = Application.Current.MainWindow as MainWindow };
                taksitDurumu.CbTaksitFilter.ItemsSource = Satış.ObservableCollectionSatışlar.ToList();
                taksitDurumu.CbTaksitFilter.SelectedItem = ((parameter as Button)?.DataContext as Satislar);
                Cv = new ObservableCollection<Taksitler>(((Satislar)taksitDurumu.CbTaksitFilter.SelectedItem).Tahsilatlar.SelectMany(t => ObservableCollectionTaksitler.Where(z => z.TahsilatID == t.TahsilatID)));
                taksitDurumu.DgTaksitDurumu.ItemsSource = Cv;
                taksitDurumu.ShowDialog();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private class TaksitKişiDeğiştirme : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => taksitDurumu.DgTaksitDurumu.ItemsSource = new ObservableCollection<Taksitler>(((Satislar)taksitDurumu.CbTaksitFilter.SelectedItem).Tahsilatlar.SelectMany(t => ObservableCollectionTaksitler.Where(z => z.TahsilatID == t.TahsilatID)));
        }

        private class TaksitKişiRaporla : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                try
                {
                    var kişi = ((Satislar)taksitDurumu.CbTaksitFilter.SelectedItem).MusteriID;
                    new Raporla("Magaza.Rapor.taksitkişiödemedurumları.frx", "MusteriID", kişi);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}