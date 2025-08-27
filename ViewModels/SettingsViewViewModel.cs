using JuanNotTheHuman.Spending.Commands;
using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Services;
using JuanNotTheHuman.Spending.Views;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.ViewModels
{
    /// <summary>
    /// A view model for the settings view.
    /// </summary>
    internal class SettingsViewViewModel : ViewModelBase
    {
        private CurrencyInfo _selectedCurrency;

        /// <summary>
        /// A collection of available currencies.
        /// </summary>
        public ObservableCollection<CurrencyInfo> Currencies { get; }

        /// <summary>
        /// The currently selected currency.
        /// </summary>
        public CurrencyInfo SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                Set(ref _selectedCurrency, value);
                ApplyCurrency(value);
            }
        }
        public ICommand BackCommand { get; }
        public ICommand ImportDatabaseCommand { get; }
        public ICommand ExportDatabaseCommand { get; }
        public ICommand ClearDatabaseCommand { get; }

        public SettingsViewViewModel()
        {
            Currencies = new ObservableCollection<CurrencyInfo>(CurrencyHelper.GetAllCurrencies());
            var currentCulture = new CultureInfo(CultureInfoHelper.Get());
            var currentCurrencySymbol = new RegionInfo(currentCulture.Name).ISOCurrencySymbol;
            SelectedCurrency = Currencies.FirstOrDefault(c => c.ISOCurrencySymbol == currentCurrencySymbol);
            BackCommand = new RelayCommand(NavigateBack);
            ImportDatabaseCommand = new RelayCommand(ImportDatabase);
            ExportDatabaseCommand = new RelayCommand(ExportDatabase);
            ClearDatabaseCommand = new RelayCommand(ClearDatabase);
        }
        private void ApplyCurrency(CurrencyInfo currency)
        {
            if (currency == null) return;
            CultureInfo.CurrentCulture = new CultureInfo(currency.CultureName);
            CultureInfoHelper.Update(currency.CultureName);
            LocalizationService.Instance.Refresh();
        }

        private void NavigateBack()
        {
            NavigationService.Navigate(new RecordsView());
        }

        private void ImportDatabase()
        {
            var ofd = CreateOpenFileDialog(LocalizationService.Instance["ImportDatabaseButton"]);
            if (ofd.ShowDialog() != true) return;

            try
            {
                DatabaseService.ImportDatabase(ofd.FileName);
            }
            catch (Exception ex)
            {
                NotificationService.ShowNotification(LocalizationService.Instance["Error"], ex.Message);
            }
        }

        private void ExportDatabase()
        {
            var sfd = CreateSaveFileDialog(LocalizationService.Instance["ExportDatabaseButton"]);
            if (sfd.ShowDialog() != true) return;

            try
            {
                DatabaseService.ExportDatabase(sfd.FileName);
                NotificationService.ShowNotification(
                    LocalizationService.Instance["Success"],
                    LocalizationService.Instance["DatabaseExportSuccessText"]);
            }
            catch (Exception ex)
            {
                NotificationService.ShowNotification(LocalizationService.Instance["Error"], ex.Message);
            }
        }

        private void ClearDatabase()
        {
            if (!NotificationService.AskConfirmation(
                LocalizationService.Instance["ClearDatabaseTitle"],
                LocalizationService.Instance["ClearDatabaseConfirmationText"]))
                return;

            try
            {
                var empty = DatabaseService.GetRecordsAsync().Result.Count == 0;
                if (empty)
                {
                    NotificationService.ShowNotification(
                        LocalizationService.Instance["Error"],
                        LocalizationService.Instance["DatabaseEmptyText"]);
                    return;
                }

                var copy = NotificationService.AskConfirmation(
                    LocalizationService.Instance["BackupDatabaseTitle"],
                    LocalizationService.Instance["BackupDatabaseText"]);

                if (copy)
                {
                    var backupDialog = CreateSaveFileDialog(LocalizationService.Instance["BackupDatabaseTitle"]);
                    if (backupDialog.ShowDialog() == true)
                        DatabaseService.ExportDatabase(backupDialog.FileName);
                }

                DatabaseService.Clear();
                NotificationService.ShowNotification(
                    LocalizationService.Instance["Success"],
                    LocalizationService.Instance["DatabaseClearedText"]);
            }
            catch (Exception ex)
            {
                NotificationService.ShowNotification(LocalizationService.Instance["Error"], ex.Message);
            }
        }
        private static OpenFileDialog CreateOpenFileDialog(string title)
        {
            return new OpenFileDialog
            {
                Title = title,
                Filter = "Database Files (*.db)|*.db|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
        }

        private static SaveFileDialog CreateSaveFileDialog(string title)
        {
            return new SaveFileDialog
            {
                Title = title,
                Filter = "Database Files (*.db)|*.db|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
        }
    }
}