using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Services;
using System.Globalization;
using System.Windows;

namespace Spending
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(CultureInfoHelper.Get());
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(CultureInfoHelper.Get());
            LocalizationService.Instance.Refresh();
            base.OnStartup(e);
        }
    }
}
