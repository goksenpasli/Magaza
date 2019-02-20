namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class Musteriler
    {
        public ICommand Ekle => new MüşteriEkleme();

        public ICommand Güncelle => new MüşteriGüncelleme();

        public ICommand MüşteriResim => new MüsteriResimleri();

        public ICommand ResimKaydet => new ResimKaydet();
        public ICommand WebcamGöster => new Webcam();
    }

    public class MüşteriEkleme : ICommand
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            MüşteriEkle(parameter);
            EkranTemizle();
        }

        private static void EkranTemizle()
        {
            Form.Gd1.Children.OfType<TextBox>().ToList().ForEach(z => z.Clear());
            Form.Gd2.Children.OfType<TextBox>().ToList().ForEach(z => z.Clear());
            Form.Gd1.Children.OfType<TextBox>().First(z => z.Focus());
        }

        private static void MüşteriEkle(object parameter)
        {
            if (!Geçerli(Form.GbMüşeriGirişi))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }
            var Button = (Musteriler)((Button)parameter)?.DataContext;

            if (new DataContext().Musteriler.Any(z => z.Tc == Button.Tc))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Bu TC Numarasında Kayıtlı Müşteri Var.", MainIcon = VistaTaskDialogIcon.Warning });
                return;
            }
            try
            {
                var dataContext = new DataContext();

                var müşteri = new Musteriler
                {
                    Aciklama = Button.Aciklama,
                    Adi = Button.Adi,
                    Adres = Button.Adres,
                    Eposta = Button.Eposta,
                    Soyadi = Button.Soyadi,
                    Tc = Button.Tc,
                    Telefon = Button.Telefon,
                    Yas = Button.Yas
                };
                dataContext.Musteriler.InsertOnSubmit(müşteri);
                dataContext.SubmitChanges();
                Satış.ObservableCollectionMüşteriler.Add(müşteri);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }

    public class Webcam : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => new Resim
        {
            Owner = Form,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            WindowState = WindowState.Normal
        }.ShowDialog();
    }
}