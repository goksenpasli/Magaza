namespace Magaza
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand ButtonFiltreler => new TaksitButtonFilters();

        private class TaksitButtonFilters : Satış, ICommand
        {
            private const int gün = 7;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                if ((string)parameter == "0")
                {
                    Cv.Filter = new Predicate<object>(item => (new DataContext().Taksitler.Any(z => z.Tahsilatlar.SatisID == ((Satislar)item).SatisID && ((z.VadeTarihi > DateTime.Now && z.VadeTarihi < DateTime.Now.AddDays(gün)) && z.TaksitOdendiMi == false))));
                    return;
                }

                if ((string)parameter == "1")
                {
                    Cv.Filter = new Predicate<object>(item => new DataContext().Taksitler.Any(z => z.Tahsilatlar.SatisID == ((Satislar)item).SatisID && (z.VadeTarihi < DateTime.Now && z.TaksitOdendiMi == false)));
                    return;
                }

                if ((string)parameter == "2")
                {
                    Cv.Filter = new Predicate<object>(item => ((Satislar)item).Tahsilatlar.Any(z => z.TahsilatBittiMi == true));
                    return;
                }

                if ((string)parameter == "4")
                {
                    Cv.Filter = new Predicate<object>(item => ((Satislar)item).PesinMi == false);
                    return;
                }
                if ((string)parameter == "5")
                {
                    Form.DtBaşlangıç.SelectedDate = Form.DtBitiş.SelectedDate = DateTime.Today;
                    return;
                }
            }
        }
    }
}