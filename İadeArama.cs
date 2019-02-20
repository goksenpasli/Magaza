using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Magaza
{
    public partial class MusteriIadeler
    {
        private string _MüsteriIadeFiltre;
        public ICollectionView CvMusteriIadeler { get; } = CollectionViewSource.GetDefaultView(ObservableCollectionMüşteriİadeler);

        public string MüsteriIadeFiltre
        {
            get => _MüsteriIadeFiltre;

            set
            {
                if (_MüsteriIadeFiltre != value)
                {
                    _MüsteriIadeFiltre = value;
                    MüsteriIadeFiltreler();
                }
            }
        }

        public void MüsteriIadeFiltreler() => CvMusteriIadeler.Filter = new Predicate<object>(item => ((MusteriIadeler)item).Musteriler.Adi.StartsWith(MüsteriIadeFiltre, StringComparison.Ordinal));
    }
}