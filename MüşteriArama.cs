using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Magaza
{
    public class MüşteriArama : Satış
    {
        private string _MüsteriFiltre;

        public ICollectionView CvMusteriler { get; } = CollectionViewSource.GetDefaultView(ObservableCollectionMüşteriler);

        public string MüsteriFiltre
        {
            get => _MüsteriFiltre;

            set
            {
                if (_MüsteriFiltre != value)
                {
                    _MüsteriFiltre = value;
                    MüsteriFiltreler();
                }
            }
        }

        public void MüsteriFiltreler() => CvMusteriler.Filter = new Predicate<object>(item => ((Musteriler)item).Adi.StartsWith(MüsteriFiltre, StringComparison.Ordinal));
    }
}