namespace Magaza
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values.All(z => z != null);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value;
    }

    public class BooleanOrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values.OfType<IConvertible>().Any(System.Convert.ToBoolean);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class BooltoVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ByteArrayToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArrayImage && byteArrayImage.Length > 0)
            {
                var bitmapImg = new BitmapImage();

                using (var stream = new MemoryStream(byteArrayImage))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    bitmapImg.BeginInit();
                    bitmapImg.DecodePixelHeight = 384;
                    bitmapImg.StreamSource = stream;
                    bitmapImg.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImg.EndInit();
                    if (bitmapImg.CanFreeze)
                        bitmapImg.Freeze();

                    return bitmapImg;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }

    public class ElementToDisabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value == 1 ? 0 : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }

    public class InttoVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ItemToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return !(value as int? < 1);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class İadeGünGeçtiMiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (value as DateTime?) > DateTime.Today && (value as DateTime?) <= DateTime.Today.AddDays(1).AddSeconds(-1);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class KdvValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var alışFiyat = System.Convert.ToDouble(values[0]);
            var kdv = System.Convert.ToDouble(values[1]);
            var kdvsizFiyat = (alışFiyat / (kdv + 100)) * 100;
            return kdvsizFiyat.ToString("C");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    }

    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ProgressValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.All(z => z != null))
            {
                var taksitToplam = System.Convert.ToDouble(values[0]);
                var pesinOdenen = System.Convert.ToDouble(values[1]);
                var toplamTutar = System.Convert.ToDouble(values[2]);
                return ((taksitToplam + pesinOdenen) / toplamTutar) * 100;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class SıfırStokConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToInt32(value) == 0;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    //public class TaksitRectangleConverter : IValueConverter
    //{
    //    private const int gün = 7;
    //    private readonly Brush LightGreen = new SolidColorBrush(Colors.LightGreen);
    //    private readonly Brush Magenta = new SolidColorBrush(Colors.Magenta);
    //    private readonly Brush Red = new SolidColorBrush(Colors.Red);
    //    private readonly Brush Yellow = new SolidColorBrush(Colors.Yellow);
    //    private readonly System.Collections.ObjectModel.ObservableCollection<Satislar> observableCollectionSatışlar = Satış.ObservableCollectionSatışlar;
    //    private readonly System.Collections.ObjectModel.ObservableCollection<Taksitler> observableCollectionTaksitler = TaksitDurumları.ObservableCollectionTaksitler;

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        try
    //        {
    //            var taksitliolan = observableCollectionSatışlar.Any(z => (z.SatisID == System.Convert.ToInt32(value) && (z.PesinMi == false)));
    //            var taksityanaşan = observableCollectionTaksitler.Any(z => (z.Tahsilatlar.SatisID == System.Convert.ToInt32(value) && (z.VadeTarihi > DateTime.Now && z.VadeTarihi < DateTime.Now.AddDays(gün)) && z.TaksitOdendiMi == false));
    //            var taksitgeçen = observableCollectionTaksitler.Any(z => (z.Tahsilatlar.SatisID == System.Convert.ToInt32(value) && z.VadeTarihi < DateTime.Now && z.TaksitOdendiMi == false));
    //            var tümtaksitleribiten = observableCollectionTaksitler.Where(z => z.Tahsilatlar.SatisID == System.Convert.ToInt32(value)).All(z => z.TaksitOdendiMi == true || z.Tahsilatlar.PesinOdenen == z.Tahsilatlar.ToplamTutar);

    //            if (tümtaksitleribiten && (string)parameter == "1")
    //            {
    //                LightGreen.Freeze();
    //                return LightGreen;
    //            }
    //            if (taksitgeçen && (string)parameter == "2")
    //            {
    //                Red.Freeze();
    //                return Red;
    //            }
    //            if (taksityanaşan && (string)parameter == "3")
    //            {
    //                Yellow.Freeze();
    //                return Yellow;
    //            }
    //            if (taksitliolan && (string)parameter == "4")
    //            {
    //                Magenta.Freeze();
    //                return Magenta;
    //            }

    //            return Brushes.Transparent;
    //        }
    //        catch
    //        {
    //            return DependencyProperty.UnsetValue;
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    //}

    public class ToptancıBorçConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) > 0;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class VisibilityToNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value is Visibility) ? (Visibility)value == Visibility.Visible : Binding.DoNothing;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value is bool) ? (bool?)value == true ? Visibility.Visible : Visibility.Collapsed : Binding.DoNothing;
    }
}