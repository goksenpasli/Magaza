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

    public class MüşteriGüncelleme : ICommand
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;
       
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => MüşteriGüncelle(parameter);

        private static void MüşteriGüncelle(object parameter)
        {
            if (!Geçerli(Form.DgMüşteriler))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }

            try
            {
                var dataContext = new DataContext();

                var Button = (parameter as Button)?.DataContext as Musteriler;
                var güncellenecek = dataContext.Musteriler.FirstOrDefault(z => z.MusteriID == Button.MusteriID);

                güncellenecek.Aciklama = Button.Aciklama;
                güncellenecek.Adi = Button.Adi;
                güncellenecek.Adres = Button.Adres;
                güncellenecek.Eposta = Button.Eposta;
                güncellenecek.Soyadi = Button.Soyadi;
                güncellenecek.Tc = Button.Tc;
                güncellenecek.Telefon = Button.Telefon;
                güncellenecek.Yas = Button.Yas;
                dataContext.SubmitChanges();
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Müşteri Güncellendi.", MainIcon = VistaTaskDialogIcon.Information });
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}