namespace Magaza
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class Satış
    {
        public static bool SatışYapılabilir => new DataContext().Ayarlar.FirstOrDefault(z => z.AyarID == 1).SatisYapilabilirMi == true;

        public ICommand Giriş => new ListeyeEkle();

        public ICommand HızlıGiriş => new HızlıListeyeEkle();

        public static void EkranTemizleme()
        {
            Form.GdSatışlar.IsEnabled = true;
            Form.DisableCloseButton(false);
            Form.BusyIndicator.IsBusy = false;
            Form.CbÖdemeTipi.SelectedIndex = 0;
            Form.TxAdet.Text = "0";
            Form.TxAçıklama.Text = "";
            Form.DuDToplamTutar.Value = 0;
            Form.TxUrunID.Text = "";
            Form.TxUrunID.Focus();
        }

        private class HızlıListeyeEkle : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                if (int.TryParse(Form.TxUrunID.Text, out int ÜrünID))
                {
                    HızlıSatışEkle(ÜrünID);
                    EkranTemizleme();
                }
            }

            private static void HızlıSatışEkle(int ÜrünID)
            {
                try
                {
                    if (!SatışYapılabilir)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Satış Yapamazsınız.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    if ((Urunler)Form.CbÜrün.SelectedItem == null)
                    {
                        return;
                    }
                    Form.CbMüşteriler.SelectedIndex = -1;
                    Form.TxAdet.Text = "1";
                    Form.CbÖdemeTipi.SelectedIndex = 0;

                    var Satış = Form.GbSatışlar.DataContext as Satislar;
                    var ÜrünMiktarı = new DataContext().Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar;
                    if (ÜrünMiktarı < Satış.Adet)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Stokta Kalan Üründen Daha Fazla Ürün Satamazsın.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    var Satışlar = new Satislar
                    {
                        Adet = Satış.Adet,
                        MusteriID = ((Musteriler)Form.CbMüşteriler.SelectedItem)?.MusteriID,
                        UrunlerUrunID = ((Urunler)Form.CbÜrün.SelectedItem).UrunID,
                        TahsilatTarih = Form.DtTahsilatTarihi.Value,
                        PesinMi = Form.CbÖdemeTipi.SelectedIndex == 0
                    };

                    var Tahsilat = new Tahsilatlar
                    {
                        PesinOdenen = (Form.GbTahsilatlar.DataContext as Tahsilatlar).PesinOdenen,
                        ToplamTutar = (Form.GbTahsilatlar.DataContext as Tahsilatlar).ToplamTutar,
                        Aciklama = (Form.GbTahsilatlar.DataContext as Tahsilatlar).Aciklama
                    };
                    Tahsilat.TahsilatBittiMi = Tahsilat.PesinOdenen == Tahsilat.ToplamTutar;

                    Satışlar.Tahsilatlar.Add(Tahsilat);
                    var dataContext = new DataContext();
                    dataContext.Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= (float)Satış.Adet;
                    dataContext.Satislar.InsertOnSubmit(Satışlar);
                    dataContext.SubmitChanges();

                    ObservableCollectionÜrünler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= (float)Satış.Adet;
                    ObservableCollectionSatışlar.Add(Satışlar);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private class ListeyeEkle : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => SatışEkle();

            private static void SatışEkle()
            {
                try
                {
                    var Satış = Form.GbSatışlar.DataContext as Satislar;
                    var ÜrünID = ((Urunler)Form.CbÜrün.SelectedItem).UrunID;
                    var ÜrünMiktarı = new DataContext().Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar;
                    if (!Geçerli(Form.GbSatışlar) || !Geçerli(Form.GbTahsilatlar))
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                        return;
                    }

                    if (!SatışYapılabilir)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Satış Yapamazsınız.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    if (ÜrünMiktarı < Satış.Adet)
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Stokta Kalan Üründen Daha Fazla Ürün Satamazsın.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    Form.GdSatışlar.IsEnabled = false;
                    Form.DisableCloseButton(true);
                    Form.BusyIndicator.IsBusy = true;

                    var Satışlar = new Satislar
                    {
                        Adet = Satış.Adet,
                        MusteriID = ((Musteriler)Form.CbMüşteriler.SelectedItem)?.MusteriID,
                        UrunlerUrunID = ((Urunler)Form.CbÜrün.SelectedItem).UrunID,
                        TahsilatTarih = Form.DtTahsilatTarihi.Value,
                        PesinMi = Form.CbÖdemeTipi.SelectedIndex == 0
                    };

                    var Tahsilat = new Tahsilatlar
                    {
                        PesinOdenen = (Form.GbTahsilatlar.DataContext as Tahsilatlar).PesinOdenen,
                        ToplamTutar = (Form.GbTahsilatlar.DataContext as Tahsilatlar).ToplamTutar,
                        Aciklama = (Form.GbTahsilatlar.DataContext as Tahsilatlar).Aciklama
                    };
                    Tahsilat.TahsilatBittiMi = Tahsilat.PesinOdenen == Tahsilat.ToplamTutar;

                    TaksitliSatış(Tahsilat);

                    Satışlar.Tahsilatlar.Add(Tahsilat);
                    var dataContext = new DataContext();
                    dataContext.Urunler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= (float)Satış.Adet;
                    dataContext.Satislar.InsertOnSubmit(Satışlar);
                    Task.Factory.StartNew(() => dataContext.SubmitChanges()).ContinueWith(task =>
                    {
                        ObservableCollectionÜrünler.FirstOrDefault(z => z.UrunID == ÜrünID).KalanMiktar -= (float)Satış.Adet;
                        ObservableCollectionSatışlar.Add(Satışlar);
                        EkranTemizleme();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private static void TaksitliSatış(Tahsilatlar Tahsilat)
            {
                if (Form.CbÖdemeTipi.SelectedIndex == 1)
                {
                    for (int i = 0; i < Convert.ToInt32(Form.TxTaksit.Text); i++)
                    {
                        var Taksitler = new Taksitler
                        {
                            VadeTarihi = Form.DtVadeTarihi.SelectedDate.Value.AddMonths(i * (int)Form.IuPÖdemeAralığı.Value),
                            TaksitBedeli = Convert.ToSingle(Form.TxTaksitTutarı.Text),
                            TaksitOdendiMi = false,
                            Sira = i + 1
                        };
                        Tahsilat.Taksitler.Add(Taksitler);
                        TaksitDurumları.ObservableCollectionTaksitler.Add(Taksitler);
                    }
                }
            }
        }
    }
}