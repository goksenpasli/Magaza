namespace Magaza
{
    using FastReport.Data;
    using FastReport.Utils;
    using LogicNP.CryptoLicensing;
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public partial class App : Application
    {
        public App()
        {
            RegisteredObjects.AddConnection(typeof(SqlCeDataConnection));
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.KeyDownEvent, new KeyEventHandler(KeyDown));
            EventManager.RegisterClassHandler(typeof(ComboBox), UIElement.KeyDownEvent, new KeyEventHandler(KeyDown));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Magaza";
            new Mutex(true, appName, out bool createdNew);
            //A();

            if (!createdNew)
            {
                MessageBox.Show(
                    "Bu Uygulama Zaten Çalışıyor. Eğer Ekranda Uygulama Görmüyorsanız Görev Yöneticisini Kontrol Edin.",
                    "Mağaza", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Current.Shutdown();
                return;
            }
            
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }

        private static void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) MoveToNextUiElement(e);
        }

        private static void MoveToNextUiElement(RoutedEventArgs e)
        {
            const FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
            var request = new TraversalRequest(focusDirection);
            var elementWithFocus = Keyboard.FocusedElement as UIElement;
            if (elementWithFocus == null) return;
            if (elementWithFocus.MoveFocus(request)) e.Handled = true;
        }

        private void A()
        {
            var A = new CryptoLicense
            {
                StorageMode = LicenseStorageMode.ToRegistry
            };

            A.ValidationKey = "AAAGgAGk/ESIIO4iJmXxVD/yH4xiLHGJIczm6SCvCAW157h/ew7tiYv/WJW0ug9nic4mf6y5GRywt0rBrFBL80Rb5otI+3AM79U669OUUIcdgP0tqaNSMUbAbLqXn6E/jispyo+R1LyUeKhNgk30iNJnto3C9LP6ASrQNA7jDWSQLWyrw94ztXc2vXSzzzpaRD0Jb3zWEqI9YY7QapnHMa/v0XI7pvK6aq46zgeZUP90yDbiHZjL7GU7R0qJOz8p3zI+6XPPKBohdfin9DcZ6QPJr9utB39wIJU6Mdx4i0acabWCGo1QeAmdUJ8Hvsc2EZ5Dpw0fFt2VfET4upf48aVpsx810oFJb4W70Y7+li5IO8Th9vcREF7OGaB1UasOppI5LCSEdgkv48r9PGldmkE58nAKjEBSp3MNVvhONMZrMPmozU/8YXttYEfmOxA7+5AaDLzJyJV5S38XC+4/0r7jOnezUggFGqjJ4gJIRXFHVMCcc8cRDQojYWzmKYloNF+2FlUDAAEAAQ==";
            if (!A.Load())
            {
                A.LicenseCode = "FgIkgTXjr2eZvNMBHgABbu29HnCHi267VJes6C+npz2IlKXxf3y6jv5STVfbfl6rU0s5pSByRT4ZclC9HYDwFg9TkLeIbowTpMNu+EFBVTILkovs7LfCIizYUZsfriR0sArMQCPzk5gP2dOdCK3Z+AyuovflkfissyJ+KAjDBYshohQ4yZdHIjPUhxA+TLc6mXv+GQTyrfFK/M1AFbJzj1km0UoeMhjRZ9SI2kh3bv5/QI8o18h9Fysa3aFRcVlGIi1ADHZGbcQK8XFzbicIyh1IVPoSRNtMzE3GeOQdzPL6YybM4+YQNIC+/yodUXbGUr9ldkMqTK7UoYtgTrTOgqJTOD/rPQrRQe4q5A40ZMSXtGu06tiZl+rfGzwlt9Aaalx8DGkV7Aixy2MGDNIxkVxIsVdmSP/BXLs9Qp8lPWpAGHG0nYZZr8iGNa9tpZ27lypQFrJdyrlYhL72vhcNlDSleR7Zn3AvyGJIvkKmzXwu/0f5bW/DAnhY7N49TmsR8LEjl7VHUallQ5ONI7iZ";
                A.Save();
            }

            if (!A.ShowEvaluationInfoDialog("Satış", "https://www.facebook.com/profile.php?id=100002547964778"))
            {
                Shutdown();
            }
            A.Dispose();
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var t = args.LoadedAssembly.GetLoadedModules();

            if (t[0].Name.Substring(t[0].Name.Length - 3, 3) == "exe")
                Shutdown();
        }
    }
}