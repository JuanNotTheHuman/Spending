using JuanNotTheHuman.Spending.Views;
using System.Windows.Controls;

namespace JuanNotTheHuman.Spending.Services
{
    /**
     * <summary>
     * A service for navigating between pages in the application.
     * </summary>
     */
    internal static class NavigationService
    {
        /**
         * <summary>
         * Navigates to the specified page.
         * </summary>
         * <param name="page">The page to navigate to.</param>
         */
        public static void Navigate(Page page)
        {
            (App.Current.MainWindow as MainWindow)?.MainFrame.Navigate(page);
        }
        /**
         * <summary>
         * Navigates to previous page in the navigation history if available.
         * </summary>
         */
        public static void GoBack()
        {
            if ((App.Current.MainWindow as MainWindow)?.MainFrame.CanGoBack == true)
            {
                (App.Current.MainWindow as MainWindow)?.MainFrame.GoBack();
            }
        }
    }
}
