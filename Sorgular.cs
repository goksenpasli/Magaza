namespace Magaza
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public partial class MusteriIadeler
    {
        public static ObservableCollection<MusteriIadeler> ObservableCollectionMüşteriİadeler { get; set; } = new ObservableCollection<MusteriIadeler>(from x in new DataContext().MusteriIadeler select x);

        public static ObservableCollection<Musteriler> ObservableCollectionMüşteriler { get; set; } = Satış.ObservableCollectionMüşteriler;
    }

    public partial class Satış : Satislar
    {
        public static ObservableCollection<Musteriler> ObservableCollectionMüşteriler { get; set; } = new ObservableCollection<Musteriler>(from x in new DataContext().Musteriler select x);

        public static ObservableCollection<Satislar> ObservableCollectionSatışlar { get; set; } = new ObservableCollection<Satislar>(from x in new DataContext().Satislar select x);

        public static ObservableCollection<Urunler> ObservableCollectionÜrünler { get; set; } = new ObservableCollection<Urunler>(from x in new DataContext().Urunler select x);

        public static ObservableCollection<Urunler> ÜrünlerFilteredObservableCollection { get; set; } = new ObservableCollection<Urunler>(from x in new DataContext().Urunler select x);
    }

    public class Tahsilat : Tahsilatlar
    {
        public static ObservableCollection<Tahsilatlar> ObservableCollectionTahsilatlar { get; set; } = new ObservableCollection<Tahsilatlar>(from x in new DataContext().Tahsilatlar select x);
    }

    public partial class TaksitDurumları : Taksitler
    {
        public static ObservableCollection<Taksitler> ObservableCollectionTaksitler { get; set; } = new ObservableCollection<Taksitler>(from x in new DataContext().Taksitler select x);
    }

    public partial class Toptancilar
    {
        public static ObservableCollection<Toptancilar> ObservableCollectionToptancılar { get; set; } = new ObservableCollection<Toptancilar>(from x in new DataContext().Toptancilar select x);
    }

    public partial class UrunKategorileri
    {
        public static ObservableCollection<UrunKategorileri> ObservableCollectionÜrünKategorileri { get; set; } = new ObservableCollection<UrunKategorileri>(from x in new DataContext().UrunKategorileri select x);
    }

    public partial class Urunler
    {
        public static ObservableCollection<UrunGruplari> ObservableCollectionÜrünGrupları { get; set; } = new ObservableCollection<UrunGruplari>(from x in new DataContext().UrunGruplari select x);
    }
}