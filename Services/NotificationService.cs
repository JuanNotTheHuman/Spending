using System.Windows;

namespace JuanNotTheHuman.Spending.Services
{
    /**
     * <summary>
     * A service for displaying notifications.
     * </summary>
     */
    internal static class NotificationService
    {
        /**
         * <summary>
         * A method to show a notification with a title and message.
         * </summary>
         */
        public static void ShowNotification(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /**
         * <summary>
         * A method to show a confirmation dialog with a title and message.
         * </summary>
         */
        public static bool AskConfirmation(string title, string message)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
