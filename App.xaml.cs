using Spending.Helpers;
using Spending.Services;
using Spending.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(ApplicationCultureInfoHelper.Get());
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(ApplicationCultureInfoHelper.Get());
            LocalizationService.Instance.Refresh();
            base.OnStartup(e);
        }
    }
}
