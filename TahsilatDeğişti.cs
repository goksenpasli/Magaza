namespace Magaza
{
    using System;
    using System.Windows.Input;
    using TaskDialogInterop;

    public partial class Tahsilatlar
    {
        private class TahsilatDeğişti : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                KişiKontrol();
                TaksitKontrol();
            }

            private static void KişiKontrol()
            {
                if (Form.CbÖdemeTipi.SelectedIndex == 1 && Form.CbMüşteriler.SelectedIndex == -1)
                {
                    Form.CbÖdemeTipi.SelectedIndex = 0;
                    TaskDialog.Show(new TaskDialogOptions { Owner = Form, Title = "Satış", MainInstruction = "Taksitli Satışlarda Müşteri Seçmeniz Gerekir.", MainIcon = VistaTaskDialogIcon.Warning });
                }
            }

            private static void TaksitKontrol()
            {
                Form.DudPeşinÖdenen.Value = (Form.CbÖdemeTipi.SelectedIndex == 0) ? Form.DuDToplamTutar.Value : 0;
                if (Form.CbÖdemeTipi.SelectedIndex == 0)
                {
                    Form.TxFaiz.IsEnabled = false;
                    Form.TxTaksit.IsEnabled = false;
                    Form.DtVadeTarihi.IsEnabled = false;
                    Form.IuPÖdemeAralığı.IsEnabled = false;
                    Form.TxFaiz.Text = "0";
                    Form.TxTaksit.Text = "0";
                    Form.TxTaksitTutarı.Text = "0";
                    Form.IuPÖdemeAralığı.Text = "1";
                    Form.DtVadeTarihi.SelectedDate = Form.DtTahsilatTarihi.Value;
                }
                else
                {
                    Form.TxTaksit.Text = "";
                    Form.TxFaiz.IsEnabled = true;
                    Form.TxTaksit.IsEnabled = true;
                    Form.DtVadeTarihi.IsEnabled = true;
                    Form.IuPÖdemeAralığı.IsEnabled = true;
                    Form.DtVadeTarihi.SelectedDate = Form.DtTahsilatTarihi.Value.Value.AddMonths(1);
                }
            }
        }
    }
}