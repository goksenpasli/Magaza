namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public class ToptancıGüncelle : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Güncelle(parameter);

        private static void Güncelle(object parameter)
        {
            if (!Geçerli(Form.GbToptancılar))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }
            try
            {
                var Button = (parameter as Button)?.DataContext as Toptancilar;
                var dataContext = new DataContext();

                var güncellenecek = dataContext.Toptancilar.FirstOrDefault(z => z.ToptanciID == Button.ToptanciID);
                güncellenecek.Adi = Button.Adi;
                güncellenecek.Adres = Button.Adres;
                güncellenecek.Aciklama = Button.Aciklama;
                güncellenecek.MalAlimTarihi = Button.MalAlimTarihi;
                güncellenecek.Telefon = Button.Telefon;
                güncellenecek.ToptanciyaOlanBorc = Button.ToptanciyaOlanBorc;
                güncellenecek.VergiNo = Button.VergiNo;

                dataContext.SubmitChanges();
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Toptancı Güncellendi", MainIcon = VistaTaskDialogIcon.Information });
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}