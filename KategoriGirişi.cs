namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public class KategoriGirişi : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Ekle(parameter);

        private static void Ekle(object parameter)
        {
            if (!Geçerli(Form.GbÜrünKategorileri))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                return;
            }

            var Button = (parameter as Button)?.DataContext as UrunKategorileri;
            if (new DataContext().UrunKategorileri.Any(z => z.KategoriAdi == Button.KategoriAdi))
            {
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Bu İsimde Kategori Var.", MainIcon = VistaTaskDialogIcon.Warning });
                return;
            }
            try
            {
                var dataContext = new DataContext();

                var kategori = new UrunKategorileri
                {
                    KategoriAdi = Button.KategoriAdi
                };
                dataContext.UrunKategorileri.InsertOnSubmit(kategori);
                dataContext.SubmitChanges();
                UrunKategorileri.ObservableCollectionÜrünKategorileri.Add(kategori);
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Kategori Eklendi", MainIcon = VistaTaskDialogIcon.Information });
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }

    public partial class UrunKategorileri
    {
        public ICommand Ekle => new KategoriGirişi();
    }
}