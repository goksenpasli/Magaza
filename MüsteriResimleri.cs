namespace Magaza
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using TaskDialogInterop;
    using static Magaza.Doğrula;

    public class MüsteriResimleri : ICommand
    {
        public static readonly int Boy = 960;
        private const int En = 1280;
        private const int Kalite = 75;

        public event EventHandler CanExecuteChanged;

        public static MemoryStream Encoder(BitmapFrame bmp)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(bmp);
            encoder.QualityLevel = Kalite;
            var stream = new MemoryStream();
            encoder.Save(stream);
            return stream;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false,
                Filter = "Resim Dosyaları (*.jpg;*.bmp;*.png;*.gif:*.tif)|*.jpg;*.bmp;*.png;*.gif:*.tif"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var bmp = BitmapFrame.Create(new Uri(openFileDialog.FileName));
                    if (bmp?.PixelWidth * bmp?.PixelHeight > En * Boy)
                    {
                        var result = TaskDialog.Show(new TaskDialogOptions
                        {
                            Owner = Form,
                            Title = "Satış",
                            MainInstruction = $"Veritabanının Gereksiz Şişmesini Önlemek ve Menülerin Açılmasını Yavaşlatmamak İçin {En}x{Boy} = {En * Boy} Pikselden Büyük Resim Yükleyemezsiniz.",
                            ExpandedInfo = "Yüklenmek İstenen Resim " + (bmp.PixelWidth * bmp.PixelHeight).ToString() + " Piksel Resmi Küçültüp Tekrar Deneyin.",
                            MainIcon = VistaTaskDialogIcon.Warning,
                            CommandButtons = new string[] { "RESMİ KÜÇÜLT VE MASAÜSTÜNE KAYDET.", "KAPAT" },
                        });
                        if (result.CommandButtonResult == 0)
                        {
                            if (bmp.PixelWidth > bmp.PixelHeight)
                                new ImageResizing(openFileDialog.FileName).Resize(En, 0).Quality(Kalite).Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + openFileDialog.SafeFileName, true);
                            else
                                new ImageResizing(openFileDialog.FileName).Resize(0, Boy).Quality(Kalite).Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + openFileDialog.SafeFileName, true);
                        }
                        return;
                    }

                    using (MemoryStream stream = Encoder(bmp))
                    {
                        var Button = (parameter as Button)?.DataContext as Musteriler;
                        var dataContext = new DataContext();
                        var güncellenecek = dataContext.Musteriler.FirstOrDefault(z => z.MusteriID == Button.MusteriID);
                        güncellenecek.Resim = Button.Resim = stream.ToArray();
                        dataContext.SubmitChanges();
                        Satış.ObservableCollectionMüşteriler.FirstOrDefault(z => z.MusteriID == Button.MusteriID).Resim = stream.ToArray();
                        
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }

    public partial class Satış
    {
        public ICommand ResimKaydet => new ResimKaydet();

        public ICommand ResimSil => new MüşteriResimSil();
    }
}