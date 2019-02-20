namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand ListeSilme => new ListeSil();

        private class ListeSil : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                try
                {
                    var Button = (Satislar)((Button)parameter)?.DataContext;
                    var Ad = Button.Musteriler == null ? "İsimsiz" : Button.Musteriler.Adi;
                    var Soyad = Button.Musteriler == null ? "İsimsiz" : Button.Musteriler.Soyadi;

                    if (MessageBox.Show($"{Ad} {Soyad} Kişisine Yapılan {Button.Adet} {Button.Urunler.OlcuBirimi} {Button.Urunler.UrunAdi} Satışı Silmek İstiyor Musun? Satış Silinince Bu Satışa İlişkin Peşin, Taksit ve İade Verileri de Silinecektir.",
                    "Satış", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    var dataContext = new DataContext();

                    var silinecek = dataContext.Satislar.FirstOrDefault(x => x.SatisID == Button.SatisID);
                    dataContext.Satislar.DeleteOnSubmit(silinecek);
                    dataContext.Urunler.FirstOrDefault(z => z.UrunID == Button.Urunler.UrunID).KalanMiktar += (float)Button.Adet;
                    dataContext.SubmitChanges();
                    ObservableCollectionÜrünler.FirstOrDefault(z => z.UrunID == Button.Urunler.UrunID).KalanMiktar += (float)Button.Adet;
                    ObservableCollectionSatışlar.Remove(Button);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}