namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class Urunler
    {
        private class PopupHızlıSatış : Satış, ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => HızlıSatışEkle(((Urunler)parameter).UrunID, parameter);

            private static void HızlıSatışEkle(int ÜrünID, object parameter)
            {
                try
                {
                    const int SatılanAdet = 1;

                    var ÜrünMiktarı = new DataContext().Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar;
                    if (ÜrünMiktarı < SatılanAdet)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Stokta Kalan Üründen Daha Fazla Ürün Satamazsın.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    var Satışlar = new Satislar
                    {
                        Adet = SatılanAdet,
                        UrunlerUrunID = ÜrünID,
                        TahsilatTarih = Form.DtTahsilatTarihi.Value,
                        PesinMi = true
                    };

                    var Tahsilat = new Tahsilatlar
                    {
                        PesinOdenen = ((Urunler)parameter).SatisFiyat,
                        ToplamTutar = ((Urunler)parameter).SatisFiyat,
                    };
                    Tahsilat.TahsilatBittiMi = Tahsilat.PesinOdenen == Tahsilat.ToplamTutar;

                    Satışlar.Tahsilatlar.Add(Tahsilat);
                    var dataContext = new DataContext();
                    dataContext.Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= SatılanAdet;
                    dataContext.Satislar.InsertOnSubmit(Satışlar);
                    dataContext.SubmitChanges();

                    ObservableCollectionÜrünler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= SatılanAdet;
                    ObservableCollectionSatışlar.Add(Satışlar);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}