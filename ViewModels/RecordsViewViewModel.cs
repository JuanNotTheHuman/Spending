using JuanNotTheHuman.Spending.Commands;
using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Models;
using JuanNotTheHuman.Spending.Services;
using JuanNotTheHuman.Spending.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.ViewModels
{
    internal class RecordsViewViewModel : ViewModelBase
    {
        private decimal _thisMonthNet;
        private decimal _thisMonthIncome;
        private decimal _thisMonthExpense;
        private decimal _totalBalance;
        private RecordViewModel _tempRecord;
        private SelectableFilterCategories _selectedCategory;
        private RecordViewTabs _selectedTab = RecordViewTabs.Overview;

        public decimal ThisMonthNet { get => _thisMonthNet; set => Set(ref _thisMonthNet, value); }
        public decimal ThisMonthIncome { get => _thisMonthIncome; set => Set(ref _thisMonthIncome, value); }
        public decimal ThisMonthExpense { get => _thisMonthExpense; set => Set(ref _thisMonthExpense, value); }
        public decimal TotalBalance { get => _totalBalance; set => Set(ref _totalBalance, value); }

        public RecordViewModel TempRecord { get => _tempRecord; set => Set(ref _tempRecord, value); }
        public RecordViewTabs SelectedTab { get => _selectedTab; set => Set(ref _selectedTab, value); }

        public SelectableFilterCategories SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (Set(ref _selectedCategory, value))
                    LoadData();
            }
        }
        public ObservableCollection<MonthlyRecords> MonthlyRecords { get; }

        private IEnumerable<RecordViewModel> CurrentMonthRecords
        {
            get
            {
                return MonthlyRecords
                    .Where(mr => mr.Date.Month == DateTime.Now.Month && mr.Date.Year == DateTime.Now.Year)
                    .SelectMany(mr => mr.DailyRecords)
                    .SelectMany(dr => dr.Records);
            }
        }
        public int ThisMonthIncomeTransactionAmount => CurrentMonthRecords.Count(r => r.Type == RecordType.Income);
        public string ThisMonthIncomeTransactionText => string.Format(Resources.Resources.thisMonthIncomeTransactions, ThisMonthIncomeTransactionAmount);
        public int ThisMonthExpenseTransactionAmount => CurrentMonthRecords.Count(r => r.Type == RecordType.Expense);
        public string ThisMonthExpenseTransactionText => string.Format(Resources.Resources.thisMonthExpenseTransactions, ThisMonthExpenseTransactionAmount);
        public int TotalTransactionAmount => MonthlyRecords.SelectMany(mr => mr.DailyRecords).SelectMany(mr => mr.Records).Count();
        public string TotalTransactionText => string.Format(Resources.Resources.TotalTransactions, TotalTransactionAmount);
        public bool IsEmpty => !MonthlyRecords.Any();

        public ICommand SwitchTabCommand => new RelayCommand<string>(tab =>
        {
            CultureInfo.CurrentCulture = new CultureInfo(CultureInfoHelper.Get());
            RecordViewTabs parsed;
            if (Enum.TryParse(tab, out parsed))
                SelectedTab = parsed;
        });

        public ICommand AddRecordCommand => new RelayCommand(() =>
        {
            _ = DatabaseService.AddRecordAsync(TempRecord.GetRecord());
            TempRecord = new RecordViewModel();
            SelectedTab = RecordViewTabs.Overview;
            LoadData();
        });
        public ICommand DeleteRecordCommand => new RelayCommand<RecordViewModel>(record =>
        {
            if (NotificationService.AskConfirmation(LocalizationService.Instance["DeleteRecordTitle"], LocalizationService.Instance["DeleteRecordText"]))
            {
                _ = DatabaseService.DeleteRecordAsync(record.Id);
                LoadData();
            }
        });
        public ICommand EditRecordCommand => new RelayCommand<RecordViewModel>(record =>
        {
            TempRecord = record;
            SelectedTab = RecordViewTabs.EditRecord;
        });
        public ICommand EditRecordSubmitCommand => new RelayCommand(() =>
        {
            SelectedTab = RecordViewTabs.Overview;
            DatabaseService.EditRecordAsync(TempRecord.GetRecord()).Wait();
            LoadData();
            TempRecord = null;
        });
        public ICommand UploadImageCommand => new RelayCommand(() => TempRecord.Image = PickImage());
        public ICommand RemoveImageCommand => new RelayCommand(() => TempRecord.Image = null);

        public ICommand NavigateToSettingsCommand => new RelayCommand(() => NavigationService.Navigate(new SettingsView()));
        public RecordsViewViewModel()
        {
            MonthlyRecords = new ObservableCollection<MonthlyRecords>();
            SelectedCategory = SelectableFilterCategories.All;
            TempRecord = new RecordViewModel();
            LoadData();
        }
        private async void LoadData()
        {
            List<Record> records;
            switch (SelectedCategory)
            {
                case SelectableFilterCategories.Incomes:
                    records = await DatabaseService.GetRecordsByTypeAsync(RecordType.Income);
                    break;
                case SelectableFilterCategories.Expenses:
                    records = await DatabaseService.GetRecordsByTypeAsync(RecordType.Expense);
                    break;
                case SelectableFilterCategories.Food:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Food);
                    break;
                case SelectableFilterCategories.Transport:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Transport);
                    break;
                case SelectableFilterCategories.Entertainment:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Entertainment);
                    break;
                case SelectableFilterCategories.Health:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Health);
                    break;
                case SelectableFilterCategories.Education:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Education);
                    break;
                case SelectableFilterCategories.Shopping:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Shopping);
                    break;
                case SelectableFilterCategories.Utilities:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Utilities);
                    break;
                case SelectableFilterCategories.Other:
                    records = await DatabaseService.GetRecordsByCategoryAsync(Category.Other);
                    break;
                default:
                    records = await DatabaseService.GetRecordsAsync();
                    break;
            }
            MonthlyRecords.Clear();
            var groupedRecords = records
                .GroupBy(r => new { r.Date.Year, r.Date.Month })
                .Select(g => new MonthlyRecords
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    DailyRecords = new ObservableCollection<DailyRecords>(
                        g.GroupBy(r => r.Date.Day)
                         .Select(dr => new DailyRecords
                         {
                             Date = new DateTime(g.Key.Year, g.Key.Month, dr.Key),
                             Records = new ObservableCollection<RecordViewModel>(
                                 dr.Select(r => new RecordViewModel(r)).OrderBy(mr => mr.Date))
                         }))
                })
                .OrderBy(mr => mr.Date);
            foreach (var monthlyRecord in groupedRecords)
                MonthlyRecords.Add(monthlyRecord);
            ThisMonthNet = await DatabaseService.GetCurrentMonthNet();
            ThisMonthIncome = await DatabaseService.GetCurrentMonthIncome();
            ThisMonthExpense = await DatabaseService.GetCurrentMonthExpense();
            TotalBalance = await DatabaseService.GetTotalBalance();
            ReloadDisplay();
        }

        private static byte[] PickImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = LocalizationService.Instance["UploadImage"]
            };
            return dialog.ShowDialog() == true ? System.IO.File.ReadAllBytes(dialog.FileName) : null;
        }

        public void ReloadDisplay()
        {
            OnPropertyChanged(nameof(MonthlyRecords));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(ThisMonthNet));
            OnPropertyChanged(nameof(ThisMonthIncome));
            OnPropertyChanged(nameof(ThisMonthExpense));
            OnPropertyChanged(nameof(TotalBalance));
            OnPropertyChanged(nameof(ThisMonthIncomeTransactionAmount));
            OnPropertyChanged(nameof(ThisMonthIncomeTransactionText));
            OnPropertyChanged(nameof(ThisMonthExpenseTransactionAmount));
            OnPropertyChanged(nameof(ThisMonthExpenseTransactionText));
            OnPropertyChanged(nameof(TotalTransactionAmount));
            OnPropertyChanged(nameof(TotalTransactionText));
        }
    }
}