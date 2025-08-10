using System.ComponentModel;
using System.Resources;
using System.Threading;
namespace JuanNotTheHuman.Spending.Services
{
    /**
     * <summary>
     * A service for localization that uses a resource manager to retrieve localized strings.
     * </summary>
     */
    internal class LocalizationService : INotifyPropertyChanged
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("JuanNotTheHuman.Spending.Resources.Resources", typeof(LocalizationService).Assembly);
        /**
         * <summary>
         * Gets the singleton instance of the LocalizationService.
         * </summary>
         */
        public static LocalizationService Instance { get; } = new LocalizationService();
        public event PropertyChangedEventHandler PropertyChanged;
        /**
         * <summary>
         * Gets a localized string by its key.
         * </summary>
         * <param name="key">The key for the localized string.</param>
         * <returns>The localized string.</returns>
         */
        public string this[string key] => _resourceManager.GetString(key,Thread.CurrentThread.CurrentUICulture);
        /**
         * <summary>
         * A method to refresh the localization service, notifying any subscribers of property changes.
         * </summary>
         */
        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
