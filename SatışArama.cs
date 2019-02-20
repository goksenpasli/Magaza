namespace Magaza
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Threading;

    public partial class Satış : Satislar
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        private readonly DispatcherTimer Timer = new DispatcherTimer();

        private float _adet;

        private DateTime _başlangıç = DateTime.Now;

        private DateTime _bitiş = DateTime.Now;

        private string _filtre;

        private int? _ıD;

        private DateTime _saat = DateTime.Now;

        public new float Adet
        {
            get => _adet;
            set
            {
                _adet = value;
                ((Satislar)Form.GbSatışlar.DataContext).Adet = Adet;
                Hesaplama();
            }
        }

        public DateTime Başlangıç
        {
            get => _başlangıç;
            set
            {
                _başlangıç = value;
                SendPropertyChanged(nameof(Başlangıç));
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).TahsilatTarih >= Başlangıç && ((Satislar)item).TahsilatTarih <= Bitiş.AddDays(1).AddSeconds(-1));
            }
        }

        public DateTime Bitiş
        {
            get => _bitiş;
            set
            {
                _bitiş = value;
                SendPropertyChanged(nameof(Bitiş));
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).TahsilatTarih >= Başlangıç && ((Satislar)item).TahsilatTarih <= Bitiş.AddDays(1).AddSeconds(-1));
            }
        }

        public ICollectionView Cv { get; set; }

        public string Filtre
        {
            get => _filtre;

            set
            {
                if (_filtre != value)
                {
                    _filtre = value;
                    ComboBoxFiltreler();
                }
            }
        }

        public int? ID
        {
            get => _ıD;

            set
            {
                if (_ıD != value)
                {
                    _ıD = value;
                    Form.CbÜrün.SelectedItem = Form.CbÜrün.Items.OfType<object>().FirstOrDefault(z => ((Urunler)z).UrunID == ID);
                }
            }
        }

        public DateTime Saat
        {
            get => _saat;
            set
            {
                _saat = value;
                SendPropertyChanged(nameof(Saat));
            }
        }

        public object SatışKriter { get; set; }

        public Satış()
        {
            Timer.Tick += (object sender, EventArgs e) => Saat = DateTime.Now;
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
            Cv = CollectionViewSource.GetDefaultView(ObservableCollectionSatışlar);
            Cv.SortDescriptions.Add(new SortDescription("TahsilatTarih", ListSortDirection.Descending));
        }

        public void ComboBoxFiltreler()
        {
            var ComboboxSelectedItem = ((SatışKriter as DataGridTextColumn)?.Binding as Binding)?.Path.Path;
            if (ComboboxSelectedItem == "Musteriler.Adi")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).MusteriID != null && ((Satislar)item).Musteriler.Adi.StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }
            if (ComboboxSelectedItem == "Musteriler.Soyadi")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).MusteriID != null && ((Satislar)item).Musteriler.Soyadi.StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }
            if (ComboboxSelectedItem == "Musteriler.Tc")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).MusteriID != null && ((Satislar)item).Musteriler.Tc.StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }
            if (ComboboxSelectedItem == "Musteriler.Telefon")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).MusteriID != null && ((Satislar)item).Musteriler.Telefon.StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }
            if (ComboboxSelectedItem == "Urunler.UrunAdi")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).Urunler.UrunAdi.StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }
            if (ComboboxSelectedItem == "Adet")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).Adet.ToString().StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }

            if (ComboboxSelectedItem == "TahsilatTarih")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).TahsilatTarih.ToString().StartsWith(Filtre, StringComparison.Ordinal));
                return;
            }

            if (((SatışKriter as DataGridCheckBoxColumn)?.Binding as Binding)?.Path.Path == "PesinMi")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).PesinMi == true);
                return;
            }

            if (ComboboxSelectedItem == "Tahsilatlar/ToplamTutar")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).Tahsilatlar.All(z => z.ToplamTutar.ToString().StartsWith(Filtre, StringComparison.Ordinal)));
                return;
            }
            if (ComboboxSelectedItem == "Tahsilatlar/PesinOdenen")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).Tahsilatlar.All(z => z.PesinOdenen.ToString().StartsWith(Filtre, StringComparison.Ordinal)));
                return;
            }
            if (ComboboxSelectedItem == "TaksitToplam")
            {
                Cv.Filter = new Predicate<object>(item => ((Satislar)item).TaksitToplam.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
        }
    }
}