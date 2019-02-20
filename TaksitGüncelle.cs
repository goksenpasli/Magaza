namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class Taksitler
    {
        public ICommand Güncelle => new TaksitGüncelle();

        private class TaksitGüncelle : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => Güncelle(parameter);

            private static void Güncelle(object parameter)
            {
                try
                {
                    if (!Geçerli(TaksitDurumları.taksitDurumu.DgTaksitDurumu))
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = TaksitDurumları.taksitDurumu, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                        return;
                    }

                    var dataContext = new DataContext();
                    var Button = ((Taksitler)((Button)parameter)?.DataContext);
                    if (new DataContext().Taksitler.Where(z => z.TahsilatID == Button.Tahsilatlar.TahsilatID).Take((int)Button.Sira - 1).Any(z => z.TaksitOdendiMi == false))
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = TaksitDurumları.taksitDurumu, Title = "Satış", MainInstruction = "Daha Önceki Taksitlerden Biri Ödenmemiş Görünüyor. Önce Onu Tahsil Edin.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    var güncellenecek = dataContext.Taksitler.FirstOrDefault(z => z.TaksitID == Button.TaksitID);
                    if (Button.TahsilEdilenTaksitTutari > güncellenecek.TaksitBedeli)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = TaksitDurumları.taksitDurumu, Title = "Satış", MainInstruction = "Taksit Bedelinden Daha Yüksek Değer Giremezsin.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }
                    güncellenecek.TahsilEdilenTaksitTutari = Button.TahsilEdilenTaksitTutari;
                    güncellenecek.Aciklama = Button.Aciklama;
                    güncellenecek.TaksitOdendiMi = Button.TaksitOdendiMi = güncellenecek.TaksitBedeli == Button.TahsilEdilenTaksitTutari;
                    dataContext.Tahsilatlar.FirstOrDefault(z => z.SatisID == Button.TahsilatID).TahsilatBittiMi = Button.Tahsilatlar.Taksitler.Where(z => z.TahsilatID == güncellenecek.TahsilatID).All(z => z.TaksitOdendiMi == true);

                    dataContext.SubmitChanges();

                    CollectionViewSource.GetDefaultView(Satış.ObservableCollectionSatışlar).Refresh();

                    TaskDialog.Show(new TaskDialogOptions { Owner = TaksitDurumları.taksitDurumu, Title = "Satış", MainInstruction = "Taksit Güncellendi.", MainIcon = VistaTaskDialogIcon.Information });
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}