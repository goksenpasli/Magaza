namespace Magaza
{
    using System;
    using System.Windows.Input;
    using TaskDialogInterop;

    public partial class Tahsilatlar
    {
        private class FaizHesaplama : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => Form.CbÖdemeTipi.SelectedIndex == 1;

            public void Execute(object parameter)
            {
                if (!string.IsNullOrEmpty(Form.TxTaksit.Text) && (DateTime.MaxValue.Subtract(DateTime.Now).Days / (365.25 / 12) < Convert.ToInt32(Form.TxTaksit.Text) * Form.IuPÖdemeAralığı.Value))
                {
                    TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", ExpandedInfo = $"Taksitlerin Vadesi {DateTime.MaxValue} Tarihini Geçiyor.", MainInstruction = "Çok Yüksek Taksit Sayısı Belirtemezsin.", MainIcon = VistaTaskDialogIcon.Warning });
                    Form.TxTaksit.Text = "";
                    return;
                }
                Form.TxTaksitTutarı.Text = TaksitHesap().ToString();
            }

            private static double Hesapla(double Kalan, double FaizOrani, int TaksitSayisi)
            {
                var sonuç = Kalan * FaizOrani * Math.Pow(1 + FaizOrani, TaksitSayisi) / (Math.Pow(1 + FaizOrani, TaksitSayisi) - 1);
                if (Double.IsNaN(sonuç))
                {
                    sonuç = Kalan / TaksitSayisi;
                }
                return Math.Round(sonuç, 2);
            }

            private double TaksitHesap()
            {
                if (Form.CbÖdemeTipi.SelectedIndex == 1 && Form.TxTaksit.Text == "0")
                {
                    Form.TxTaksit.Text = "";
                }
                return Hesapla((double)((Tahsilatlar)Form.GbTahsilatlar.DataContext)?.Kalan, (double)(((Tahsilatlar)Form.GbTahsilatlar.DataContext)?.FaizOrani / 100), (int)((Tahsilatlar)Form.GbTahsilatlar.DataContext)?.TaksitSayisi);
            }
        }
    }
}