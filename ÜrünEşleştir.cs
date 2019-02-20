namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using TaskDialogInterop;

    public partial class UrunGruplari
    {
        public ICommand Eşleştir => new ÜrünEşleştir();
    }

    public class ÜrünEşleştir : ICommand
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Eşleştir();

        private static void Eşleştir()
        {
            try
            {
                if (new DataContext().UrunGruplari.Any(z => z.ToptanciID == (Form.CbGrupToptancı.SelectedItem as Toptancilar).ToptanciID && z.KategoriId == (Form.CbGrupKategori.SelectedItem as UrunKategorileri).KategoriId))
                {
                    TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Bu İsimde Kategori ve Toptancı Eşleştirmesi Zaten Yapılmış.", MainIcon = VistaTaskDialogIcon.Warning });
                    return;
                }
                var dataContext = new DataContext();

                var eşleştirme = new UrunGruplari
                {
                    ToptanciID = (Form.CbGrupToptancı.SelectedItem as Toptancilar).ToptanciID,
                    KategoriId = (Form.CbGrupKategori.SelectedItem as UrunKategorileri).KategoriId
                };
                dataContext.UrunGruplari.InsertOnSubmit(eşleştirme);
                dataContext.SubmitChanges();
                Urunler.ObservableCollectionÜrünGrupları.Add(eşleştirme);
                TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Kategori ve Toptancı Eşleştirmesi Yapıldı.", MainIcon = VistaTaskDialogIcon.Information });
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}