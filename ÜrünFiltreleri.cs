namespace Magaza
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Data;

    public partial class Urunler
    {
        private string _filtre;

        public ICollectionView Cv { get; set; } = CollectionViewSource.GetDefaultView(Satış.ObservableCollectionÜrünler);

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

        public object Kriter { get; set; }

        public void Filtreler()
        {
            var ComboboxSelectedItem = ((Kriter as DataGridTextColumn)?.Binding as Binding)?.Path.Path;
            if (ComboboxSelectedItem == "UrunID")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).UrunID.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "UrunAdi")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).UrunAdi.StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "Kdv")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).Kdv.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "ToplamMiktar")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).ToplamMiktar.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "KalanMiktar")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).KalanMiktar.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "AlisFiyat")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).AlisFiyat.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "Aciklama")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).Aciklama.StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "SatisFiyat")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).SatisFiyat.ToString().StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "UrunGruplari.Toptancilar.Adi")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).UrunGruplari.Toptancilar.Adi.StartsWith(Filtre, StringComparison.Ordinal));
            }
            if (ComboboxSelectedItem == "UrunGruplari.UrunKategorileri.KategoriAdi")
            {
                Cv.Filter = new Predicate<object>(item => ((Urunler)item).UrunGruplari.UrunKategorileri.KategoriAdi.StartsWith(Filtre, StringComparison.Ordinal));
            }
        }
    }
}