namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class MüşteriResimSil : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                var resim = parameter as Musteriler;
                if (resim == null) return;

                if (MessageBox.Show("Seçili Resmi Silmek İstiyor Musun?",
               "Satış", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }

                var dataContext = new DataContext();
                var güncellenecek = dataContext.Musteriler.FirstOrDefault(z => z.MusteriID == resim.MusteriID);
                resim.Resim = güncellenecek.Resim = null;
                dataContext.SubmitChanges();
                Satış.ObservableCollectionMüşteriler.FirstOrDefault(z => z.MusteriID == resim.MusteriID).Resim = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}