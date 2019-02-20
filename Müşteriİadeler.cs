namespace Magaza
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public partial class MusteriIadeler
    {
        private static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        private ObservableCollection<Satislar> _filteredSatışlar;

        private object _seçiliMüşteri;

        public ObservableCollection<Satislar> FilteredSatışlar
        {
            get => _filteredSatışlar;
            set
            {
                if (_filteredSatışlar != value)
                {
                    _filteredSatışlar = value;
                    SendPropertyChanged(nameof(FilteredSatışlar));
                }
            }
        }

        public ICommand MüşteriİadeEkleme => new İadeEkle();

        public ICommand MüşteriİadeRaporAl => new İadeRaporla();

        public object SeçiliMüşteri
        {
            get => _seçiliMüşteri;
            set
            {
                if (_seçiliMüşteri != value)
                {
                    _seçiliMüşteri = value;
                    FilteredSatışlar = new ObservableCollection<Satislar>(new DataContext().Satislar.Where(x => x.MusteriID == ((Musteriler)SeçiliMüşteri).MusteriID));
                }
            }
        }

        private class İadeEkle : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                try
                {
                    if (!Geçerli(Form.GbMüşteriİade))
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Tüm Alanlara Doğru Giriş Yaptığınızdan Emin Olun.", MainIcon = VistaTaskDialogIcon.Warning, ExpandedInfo = "Kırmızı Uyarı İle İşaretlenmiş Alanların Doğru Doldurulması Gereklidir." });
                        return;
                    }

                    if (new DataContext().MusteriIadeler.Any(z => (z.MusteriID == ((Musteriler)Form.CbMüşteriİade.SelectedItem).MusteriID && z.SatisID == ((Satislar)Form.CbÜrünİade.SelectedItem).SatisID)))
                    {
                        TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Bu Şatış İşlemine İlişkin Daha Önce İade Yapılmıştır. Yeniden İade Yapamazsın.", MainIcon = VistaTaskDialogIcon.Warning });
                        return;
                    }

                    var İadeler = new MusteriIadeler
                    {
                        IadeTarihi = Form.DtİadeTarihi.SelectedDate.Value,
                        IadeAdeti = (int)Form.IudMüşteriİadeAdeti.Value,
                        Aciklama = Form.TxİadeAçıklama.Text,
                        OdenenTutar = (float)Form.CuDMüşteriÖdenecekTutar.Value,
                        UrunID = ((Satislar)Form.CbÜrünİade.SelectedItem).Urunler.UrunID,
                        MusteriID = ((Musteriler)Form.CbMüşteriİade.SelectedItem).MusteriID,
                        SatisID = ((Satislar)Form.CbÜrünİade.SelectedItem).SatisID
                    };

                    var dataContext = new DataContext();
                    dataContext.Urunler.FirstOrDefault(z => z.UrunID == ((Satislar)Form.CbÜrünİade.SelectedItem).Urunler.UrunID).KalanMiktar += (int)Form.IudMüşteriİadeAdeti.Value;
                    dataContext.MusteriIadeler.InsertOnSubmit(İadeler);
                    dataContext.SubmitChanges();
                    Satış.ObservableCollectionÜrünler.FirstOrDefault(z => z.UrunID == ((Satislar)Form.CbÜrünİade.SelectedItem).Urunler.UrunID).KalanMiktar += (int)Form.IudMüşteriİadeAdeti.Value;
                    ObservableCollectionMüşteriİadeler.Add(İadeler);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private class İadeRaporla : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                try
                {
                    new Raporla("Magaza.Rapor.iadeler.frx");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}