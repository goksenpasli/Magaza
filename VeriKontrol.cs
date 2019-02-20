namespace Magaza
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class Doğrula
    {
        public static readonly MainWindow Form = Application.Current.MainWindow as MainWindow;

        public static bool Geçerli(DependencyObject parent)
        {
            if (Validation.GetHasError(parent)) return false;
            for (var i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (!Geçerli(child)) return false;
            }
            return true;
        }
    }

    public class BirdenBüyük : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim()?.Length == 0) return new ValidationResult(false, "Değer Boş Olamaz.");
            try
            {
                if (double.Parse(value.ToString()) < 1) return new ValidationResult(false, "1 veya 1 den büyük bir değer girin");
            }
            catch
            {
                return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class RegularExpressionValidation : ValidationRule
    {
        public string Expression { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                Regex regex = new Regex(Expression);
                Match match = regex.Match(value.ToString());
                if (match == null || match == Match.Empty)
                    return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
                else
                    return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
        }
    }

    public class SıfırBirArası : ValidationRule
    {
        public double Enaz { get; set; }

        public double Ençok { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim()?.Length == 0) return new ValidationResult(false, "Değer Boş Olamaz.");
            try
            {
                if (double.Parse(value.ToString()) > Ençok || double.Parse(value.ToString()) < Enaz)
                {
                    return new ValidationResult(false, string.Format("Girilen Değer {0} ile {1} Arasında Olmalıdır.", Enaz, Ençok));
                }
            }
            catch
            {
                return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class SıfırDahilSıfırdandanBüyük : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim()?.Length == 0) return new ValidationResult(false, "Değer Boş Olamaz.");
            try
            {
                if (double.Parse(value.ToString()) < 0) return new ValidationResult(false, "0 dan büyük bir değer girin");
            }
            catch
            {
                return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class SıfırdanBüyük : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim()?.Length == 0) return new ValidationResult(false, "Değer Boş Olamaz.");
            try
            {
                if (double.Parse(value.ToString()) <= 0) return new ValidationResult(false, "0 dan büyük bir değer girin");
            }
            catch
            {
                return new ValidationResult(false, "Hatalı Giriş Var Düzeltin.");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class TarihDoğrula : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                DateTime date = DateTime.Now;
                if (value != null)
                {
                    date = DateTime.Parse(value.ToString());
                    if (DateTime.Now < date || date < new DateTime(1753, 1, 1))
                    {
                        return new ValidationResult(false, "Bugünden Büyük veya 01.01.1753'den Küçük Tarih Girmeyin.");
                    }
                }
                return ValidationResult.ValidResult;
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Geçerli Tarih Değil");
            }
        }
    }

    public class TcKimlikDoğrula : ValidationRule
    {
        public static bool Rakakmmı(string tcKimlikNo) => tcKimlikNo.Aggregate(true, (current, t) => current & char.IsNumber(t));

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var tekler = 0;
            var ciftler = 0;
            if (value == null) return new ValidationResult(false, "Burayı Boş Geçmeyin.");
            var tcKimlikNo = value as string;

            if (tcKimlikNo == null) return new ValidationResult(false, "Burayı Boş Geçmeyin.");
            if (tcKimlikNo.Trim()?.Length == 0) return new ValidationResult(false, "Burayı Boş Geçmeyin.");
            try
            {
                if (tcKimlikNo.Length != 11) return new ValidationResult(false, "TC Kimlik No 11 Hanelidir");
                if (!Rakakmmı(tcKimlikNo)) return new ValidationResult(false, "TC Kimlik No Sadece Rakamlardan Oluşur");
                if (tcKimlikNo.Substring(0, 1) == "0") return new ValidationResult(false, "TC Kimlik İlk Hanesi 0 Olamaz");
                int i;
                for (i = 0; i < 9; i += 2) tekler += int.Parse(tcKimlikNo[i].ToString());
                for (i = 1; i < 8; i += 2) ciftler += int.Parse(tcKimlikNo[i].ToString());
                var k10 = (((tekler * 7) - ciftler) % 10).ToString();
                var k11 = ((tekler + ciftler + int.Parse(tcKimlikNo[9].ToString())) % 10).ToString();
                if (k10 == tcKimlikNo[9].ToString() && k11 == tcKimlikNo[10].ToString()) return ValidationResult.ValidResult;
                return new ValidationResult(false, "Girilen TC Kimlik Yanlış");
            }
            catch (Exception hata)
            {
                return new ValidationResult(false, hata.Message);
            }
        }
    }

    public class TxtDoğrula : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim()?.Length == 0)
                return new ValidationResult(false, "Burayı Boş Geçmeyin.");
            else
                return ValidationResult.ValidResult;
        }
    }

    public class VergiNoDoğrula : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Length != 10) return new ValidationResult(false, "Vergi Numarası Boş Olmaz ve 10 Rakamdan Oluşur.");

            var vkn = value.ToString().ToArray();
            if (!vkn.All(n => char.IsNumber(n))) return new ValidationResult(false, "Vergi Numarası Tümü Rakamdan Oluşur.");

            var lastDigit = Convert.ToInt32(vkn[9].ToString());
            int total = 0;
            for (int i = 9; i >= 1; i--)
            {
                int digit = Convert.ToInt32(vkn[9 - i].ToString());
                var v1 = ((digit + i) % 10);
                int v11 = (int)(v1 * Math.Pow(2, i)) % 9;
                if (v1 != 0 && v11 == 0) v11 = 9;
                total += v11;
            }

            total = (total % 10 == 0) ? 0 : (10 - (total % 10));
            return (total == lastDigit) ? ValidationResult.ValidResult : new ValidationResult(false, "Vergi Numarası Geçerli Değil.");
        }
    }
}