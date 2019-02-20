namespace Magaza
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;

    public partial class Toptancilar
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        private string _filtre;

        public ICollectionView Cv { get; } = CollectionViewSource.GetDefaultView(ObservableCollectionToptancılar);

        public string Filtre
        {
            get => _filtre;

            set
            {
                if (_filtre != value)
                {
                    _filtre = value;
                    Filtreler();
                }
            }
        }

        public void Filtreler() => Cv.Filter = new Predicate<object>(item => ((Toptancilar)item).Adi.StartsWith(Filtre, StringComparison.Ordinal));
    }
}