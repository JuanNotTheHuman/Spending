using JuanNotTheHuman.Spending.Commands;
using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Services;
using JuanNotTheHuman.Spending.Views;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.ViewModels
{
    /**
     * <summary>
     * A view model for the settings view.
     * </summary>
     */
    internal class SettingsViewViewModel : ViewModelBase
    {
        private CurrencyInfo _selectedCurrency;
        /**
         * <summary>
         * A collection of available currencies.
         * </summary>
         */
        public ObservableCollection<CurrencyInfo> Currencies { get; }
        /**
         * <summary>
         * The currently selected currency.
         * </summary>
         */
        public CurrencyInfo SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                if (_selectedCurrency != value)
                {
                    _selectedCurrency = value;
                    OnPropertyChanged();
                    CultureInfo.CurrentCulture = new CultureInfo(value.CultureName);
                    CultureInfoHelper.Update(value.CultureName);
                    LocalizationService.Instance.Refresh();
                }
            }
        }
        /**
         * <summary>
         * Command to navigate back to the records view.
         * </summary>
         */
        public ICommand BackCommand => new RelayCommand(() =>
        {
            NavigationService.Navigate(new RecordsView());
        });
        /**
         * <summary>
         * Command to import a database file.
         * </summary>
         */
        public ICommand ImportDatabaseCommand => new RelayCommand(() =>
        {
            var ofselect = new OpenFileDialog
            {
                Title = "Select Database File",
                Filter = "Database Files (*.db)|*.db|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (ofselect.ShowDialog() == true)
            {
                try
                {
                    DatabaseService.ImportDatabase(ofselect.FileName);
                }
                catch (Exception ex)
                {
                    NotificationService.ShowNotification("Error", ex.Message);
                }
            }
        });
        /**
         * <summary>
         * Command to export the current database.
         * </summary>
         */
        public ICommand ExportDatabaseCommand => new RelayCommand(() =>
        {
            var sfd = new SaveFileDialog
            {
                Title = "Export Database File",
                Filter = "Database Files (*.db)|*.db|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    DatabaseService.ExportDatabase(sfd.FileName);
                    NotificationService.ShowNotification("Success", "Database exported successfully");
                }
                catch (Exception ex)
                {
                    NotificationService.ShowNotification("Error", ex.Message);
                }
            }
        });
        public ICommand ClearDatabaseCommand => new RelayCommand(() =>
        {
            bool confirm = NotificationService.AskConfirmation("Clear database","Are you sure you want to clear the database? This action cannot be undone.");
            if (confirm)
            {
                try
                {
                    bool empty = DatabaseService.GetRecordsAsync().Result.Count == 0;
                    if (empty)
                    {
                        NotificationService.ShowNotification("Info", "The database is already empty.");
                        return;
                    }
                    bool copy = NotificationService.AskConfirmation("Backup database", "Do you want to create a backup before clearing the database?");
                    if(copy)
                    {
                        var sfd = new SaveFileDialog
                        {
                            Title = "Backup Database File",
                            Filter = "Database Files (*.db)|*.db|All Files (*.*)|*.*",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        };
                        if (sfd.ShowDialog() == true)
                        {
                            DatabaseService.ExportDatabase(sfd.FileName);
                        }
                    }
                    DatabaseService.Clear();
                    NotificationService.ShowNotification("Success","Database cleared successfully");
                }
                catch (Exception ex)
                {
                    NotificationService.ShowNotification("Error", ex.Message);
                }
            }
        });
        public SettingsViewViewModel()
        {
            Currencies = new ObservableCollection<CurrencyInfo>(CurrencyHelper.GetAllCurrencies());
            CultureInfo currentCulture = new CultureInfo(CultureInfoHelper.Get());
            string currentCurrencySymbol = new RegionInfo(currentCulture.Name).ISOCurrencySymbol;
            SelectedCurrency = Currencies.FirstOrDefault(c => c.ISOCurrencySymbol == currentCurrencySymbol);
        }
    }
}
