using Microsoft.Win32;
using Spending.Commands;
using Spending.Helpers;
using Spending.Services;
using Spending.Views;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Spending.ViewModels
{
    internal class SettingsViewViewModel : ViewModelBase
    {
        private CurrencyInfo _selectedCurrency;
        public ObservableCollection<CurrencyInfo> Currencies { get; }
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
                    ApplicationCultureInfoHelper.Update(value.CultureName);
                    LocalizationService.Instance.Refresh();
                }
            }
        }
        public ICommand BackCommand => new RelayCommand(() =>
        {
            NavigationService.Navigate(new RecordsView());
        });
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
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        });
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
                    MessageBox.Show("Database exported successfully.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        });
        public SettingsViewViewModel()
        {
            Currencies = new ObservableCollection<CurrencyInfo>(CurrencyHelper.GetAllCurrencies());
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            string currentCurrencySymbol = new RegionInfo(currentCulture.Name).ISOCurrencySymbol;
            SelectedCurrency = Currencies.FirstOrDefault(c => c.ISOCurrencySymbol == currentCurrencySymbol);
        }
    }
}
