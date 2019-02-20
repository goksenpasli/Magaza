namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class Urunler
    {
        public ICommand Ekle => new ÜrünGirişleri();

        public ICommand HızlıSatışEkle => new PopupHızlıSatış();
    }

    public class ÜrünGirişleri : ICommand
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            ÜrünEkle(parameter);
            EkranTemizle();

        }

        private static void EkranTemizle()
        {
            Form.GdÜrünGirişleri.Children.OfType<TextBox>().ToList().ForEach(z => z.Clear());
            Form.GdÜrünGirişleri.Children.OfType<Xceed.Wpf.Toolkit.DoubleUpDown>().ToList().ForEach(z => z.Value = 0);
            Form.GdÜrünGirişleri.Children.OfType<TextBox>().First(z => z.Focus());
        }

        private static void ÜrünEkle(object parameter)
        {
            if (!Geçerli(Form.GbÜrünGirişleri))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }

            try
            {
                var dataContext = new DataContext();
                var Button = (parameter as Button)?.DataContext as Urunler;
                var kategori = new Urunler
                {
                    Aciklama = Button.Aciklama,
                    KalanMiktar = Button.KalanMiktar,
                    ToplamMiktar = Button.KalanMiktar,
                    AlisFiyat = Button.AlisFiyat,
                    UrunAdi = Button.UrunAdi,
                    Kdv = Button.Kdv,
                    SatisFiyat = Button.SatisFiyat,
                    OlcuBirimi = Button.OlcuBirimi,
                    Favori = false,
                    UrunGrupID = (Form.CbÜrünGrupları.SelectedItem as UrunGruplari)?.UrunGrupID,
                };
                dataContext.Urunler.InsertOnSubmit(kategori);
                dataContext.SubmitChanges();
                Satış.ObservableCollectionÜrünler.Add(kategori);
                Satış.ÜrünlerFilteredObservableCollection.Add(kategori);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}