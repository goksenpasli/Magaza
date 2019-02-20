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

    public partial class Urunler
    {
        public ICommand Güncelle => new ÜrünGüncellemeleri();

        public ICommand ResimKaydet => new ResimKaydet();

        public ICommand ResimSil => new ÜrünResimSil();

        public ICommand ÜrünBarkod => new ÜrünBarkodları();

        public ICommand ÜrünResim => new ÜrünResimleri();
    }

    public class ÜrünGüncellemeleri : ICommand
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => ÜrünGüncelle(parameter);

        private static void ÜrünGüncelle(object parameter)
        {
            if (!Geçerli(Form.GBÜrünlerTablosu))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }

            if (Satış.ObservableCollectionÜrünler.Count(z => z.Favori == true) > 300)
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "300 Adet Üründen Fazla Ürünü Favorilere Ekleyemezsin.", MainIcon = VistaTaskDialogIcon.Warning });
                return;
            }

            try
            {
                var dataContext = new DataContext();

                var Button = (parameter as Button)?.DataContext as Urunler;
                var güncellenecek = dataContext.Urunler.FirstOrDefault(z => z.UrunID == Button.UrunID);

                güncellenecek.Aciklama = Button.Aciklama;
                güncellenecek.KalanMiktar = Button.KalanMiktar;
                güncellenecek.AlisFiyat = Button.AlisFiyat;
                güncellenecek.UrunAdi = Button.UrunAdi;
                güncellenecek.Kdv = Button.Kdv;
                güncellenecek.SatisFiyat = Button.SatisFiyat;
                güncellenecek.OlcuBirimi = Button.OlcuBirimi;
                güncellenecek.Favori = Button.Favori;
                dataContext.SubmitChanges();

                Satış.ÜrünlerFilteredObservableCollection.FirstOrDefault(z => z.UrunID == Button.UrunID).Favori = Button.Favori;
                CollectionViewSource.GetDefaultView(Satış.ÜrünlerFilteredObservableCollection).Refresh();
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Ürün Güncellendi", MainIcon = VistaTaskDialogIcon.Information });
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}