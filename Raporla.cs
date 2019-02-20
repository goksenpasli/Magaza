namespace Magaza
{
    using FastReport;
    using FastReport.Utils;
    using System.Collections;
    using System.Windows;

    public class Raporla
    {
        public Raporla(string streamname, IEnumerable data, string name)
        {
            try
            {
                using (var bordrorapor = GetType().Assembly.GetManifestResourceStream(streamname))
                {
                    using (var language = GetType().Assembly.GetManifestResourceStream(
                        "Magaza.Rapor.Turkish.frl"))
                    {
                        using (var report = new Report())
                        {
                            Res.LoadLocale(language);
                            report.Load(bordrorapor);
                            report.SetParameterValue("Parameter", new DataContext().Connection.ConnectionString);
                            report.RegisterData(data, name);
                            Config.ReportSettings.ShowPerformance = false;
                            Config.PreviewSettings.Buttons = PreviewButtons.All & ~PreviewButtons.Edit;
                            report.Show(true);
                        }
                    }
                }
            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public Raporla(string streamname, string param, object value)
        {
            try
            {
                using (var bordrorapor = GetType().Assembly.GetManifestResourceStream(streamname))
                {
                    using (var language = GetType().Assembly.GetManifestResourceStream(
                        "Magaza.Rapor.Turkish.frl"))
                    {
                        using (var report = new Report())
                        {
                            Res.LoadLocale(language);
                            report.Load(bordrorapor);
                            report.SetParameterValue("Parameter", new DataContext().Connection.ConnectionString);
                            report.SetParameterValue(param, value);
                            Config.ReportSettings.ShowPerformance = false;
                            Config.PreviewSettings.Buttons = PreviewButtons.All & ~PreviewButtons.Edit;
                            report.Show(true);
                        }
                    }
                }
            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public Raporla(string streamname)
        {
            try
            {
                using (var bordrorapor = GetType().Assembly.GetManifestResourceStream(streamname))
                {
                    using (var language = GetType().Assembly.GetManifestResourceStream(
                        "Magaza.Rapor.Turkish.frl"))
                    {
                        using (var report = new Report())
                        {
                            Res.LoadLocale(language);
                            report.Load(bordrorapor);
                            report.SetParameterValue("Parameter", new DataContext().Connection.ConnectionString);
                            Config.ReportSettings.ShowPerformance = false;
                            Config.PreviewSettings.Buttons = PreviewButtons.All & ~PreviewButtons.Edit;
                            report.Show(true);
                        }
                    }
                }
            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}