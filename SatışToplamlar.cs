namespace Magaza
{
    using System.Linq;

    public partial class Satislar
    {
        private double _TaksitToplam;

        private double _TümToplam;

        public double TaksitToplam
        {
            get => TaksitDurumları.ObservableCollectionTaksitler.Where(z => z.TahsilatID == SatisID).Sum(z => z.TahsilEdilenTaksitTutari);

            set
            {
                if (_TaksitToplam != value)
                {
                    _TaksitToplam = value;
                    SendPropertyChanged(nameof(TaksitToplam));
                }
            }
        }

        public double TümToplam
        {
            get => TaksitToplam + (Tahsilat.ObservableCollectionTahsilatlar.FirstOrDefault(z => z.TahsilatID == SatisID)?.PesinOdenen ?? 0);

            set
            {
                if (_TümToplam != value)
                {
                    _TümToplam = value;
                    SendPropertyChanged(nameof(TümToplam));
                }
            }
        }
    }
}