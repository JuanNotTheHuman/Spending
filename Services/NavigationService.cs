using Spending.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spending.Services
{
    internal static class NavigationService
    {
        public static void Navigate(Page page)
        {
            (App.Current.MainWindow as MainWindow)?.MainFrame.Navigate(page);
        }
        public static void GoBack()
        {
            if ((App.Current.MainWindow as MainWindow)?.MainFrame.CanGoBack == true)
            {
                (App.Current.MainWindow as MainWindow)?.MainFrame.GoBack();
            }
        }
    }
}
