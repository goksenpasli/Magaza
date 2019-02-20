namespace Magaza
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand ToplamAl => new TahsilatToplamları();

        public class TahsilatToplamları : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                float toplam = 0;
                float toplampeşin = 0;
                double toplamtaksit = 0;
                float adet = 0;
                foreach (var item in parameter as IList)
                {
                    toplam += (item as Satislar)?.Tahsilatlar.FirstOrDefault()?.ToplamTutar ?? 0;
                    toplampeşin += (item as Satislar)?.Tahsilatlar.FirstOrDefault()?.PesinOdenen ?? 0;
                    adet += (item as Satislar)?.Adet ?? 0;
                    toplamtaksit += (item as Satislar)?.TaksitToplam ?? 0;
                }
                Form.TbToplam.Text = toplam.ToString("C");
                Form.TbToplamPeşinÖdenen.Text = toplampeşin.ToString("C");
                Form.TbToplamAdet.Text = adet.ToString();
                Form.TbToplamTaksitÖdenen.Text = ((float)toplamtaksit).ToString("C");
            }
        }
    }
}