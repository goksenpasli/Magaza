namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public class ToptancıEkle : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Ekle(parameter);

        private static void Ekle(object parameter)
        {
            if (!Geçerli(Form.GbToptancıGirişi))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }

            var Button = (parameter as Button)?.DataContext as Toptancilar;
            if (new DataContext().Toptancilar.Any(z => z.VergiNo == Button.VergiNo))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Bu Vergi Numarasında Kayıtlı Toptancı Var.", MainIcon = VistaTaskDialogIcon.Warning });
                return;
            }
            try
            {
                var dataContext = new DataContext();

                var kategori = new Toptancilar
                {
                    Adi = Button.Adi,
                    Adres = Button.Adres,
                    Aciklama = Button.Aciklama,
                    MalAlimTarihi = Button.MalAlimTarihi,
                    Telefon = Button.Telefon,
                    ToptanciyaOlanBorc = Button.ToptanciyaOlanBorc,
                    VergiNo = Button.VergiNo
                };
                dataContext.Toptancilar.InsertOnSubmit(kategori);
                dataContext.SubmitChanges();
                Toptancilar.ObservableCollectionToptancılar.Add(kategori);
                //MessageBox.Show("Toptancı Eklendi.", "Satış", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }

    public partial class Toptancilar
    {
        public ICommand Ekle => new ToptancıEkle();

        public ICommand Güncelle => new ToptancıGüncelle();

        public ICommand RaporAl => new ToptancıRaporAl();
    }
}