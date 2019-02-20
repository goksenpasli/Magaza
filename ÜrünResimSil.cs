namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class ÜrünResimSil : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                var resim = parameter as Urunler;
                if (resim == null) return;

                if (MessageBox.Show("Seçili Resmi Silmek İstiyor Musun?",
               "Satış", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }
                var dataContext = new DataContext();
                var güncellenecek = dataContext.Urunler.FirstOrDefault(z => z.UrunID == resim.UrunID);
                resim.Resim = güncellenecek.Resim = null;
                dataContext.SubmitChanges();
                Satış.ÜrünlerFilteredObservableCollection.FirstOrDefault(z => z.UrunID == resim.UrunID).Resim = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}