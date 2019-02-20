namespace Magaza
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public partial class Tahsilatlar
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        private double? _kalan = 0;

        private DateTime? _vadeBaşlangıçTarihi;

        public ICommand ComboboxTahsilatDeğişti => new TahsilatDeğişti();

        public ICommand DoubleUpDownDeğişti => new UpDownDeğişti();

        public ICommand FaizHesap => new FaizHesaplama();

        public double? Kalan
        {
            get => _kalan;

            set
            {
                if (_kalan != value)
                {
                    _kalan = value;
                    SendPropertyChanged(nameof(Kalan));
                }
            }
        }

        public DateTime? VadeBaşlangıçTarihi
        {
            get => _vadeBaşlangıçTarihi;
            set
            {
                if (_vadeBaşlangıçTarihi != value)
                {
                    _vadeBaşlangıçTarihi = value;
                    SendPropertyChanged(nameof(VadeBaşlangıçTarihi));
                }
            }
        }
    }
}