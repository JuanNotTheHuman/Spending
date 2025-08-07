using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Services
{
    public static class NotificationService
    {
        public static void ShowNotification(string title, string message)
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
        public static bool AskConfirmation(string title, string message)
        {
            var result = System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            return result == System.Windows.MessageBoxResult.Yes;
        }
    }
}
