namespace Magaza
{
    using System;
    using System.Windows.Data;
    using System.Windows.Input;

    public partial class Satış
    {
        public ICommand HızlıSatışFiltreler => new HızlıSatışDropDown();

        private class HızlıSatışDropDown : ICommand
        {
            static HızlıSatışDropDown() => CollectionViewSource.GetDefaultView(ÜrünlerFilteredObservableCollection).Filter = new Predicate<object>(item => ((Urunler)item).Favori == true);

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
            }
        }
    }
}