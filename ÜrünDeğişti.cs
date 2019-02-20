namespace Magaza
{
    using System;
    using System.Windows.Input;

    public partial class Satış : Satislar
    {
        public ICommand ÜrünDeğiştirme => new ÜrünDeğişti();

        private static void Hesaplama()
        {
            Form.TxUrunID.Text = (Form.CbÜrün.SelectedItem as Urunler)?.UrunID.ToString();
            Form.DuDToplamTutar.Value = (Form.GbSatışlar.DataContext as Satislar).Adet * (Form.CbÜrün.SelectedItem as Urunler)?.SatisFiyat ?? 0;
            Form.DudPeşinÖdenen.Value = (Form.CbÖdemeTipi.SelectedIndex == 0) ? Form.DuDToplamTutar.Value : 0;
        }

        private class ÜrünDeğişti : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => Hesaplama();
        }
    }
}