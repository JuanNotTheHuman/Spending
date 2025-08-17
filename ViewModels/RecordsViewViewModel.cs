using JuanNotTheHuman.Spending.Commands;
using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Models;
using JuanNotTheHuman.Spending.Services;
using JuanNotTheHuman.Spending.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.ViewModels
{
    /**
     * <summary>
     * ViewModel for the RecordsView.
     * </summary>
     */
    internal class RecordsViewViewModel : ViewModelBase
    {
        private decimal _thisMonthNet;
        private decimal _thisMonthIncome;
        private decimal _thisMonthExpense;
        private decimal _totalBalance;
        private RecordViewModel _newRecord;
        private RecordViewModel _editRecord;
        private SelectableFilterCategories _selectedCategory;
        private RecordViewTabs _selectedTab = RecordViewTabs.Overview;
        /**
         * <summary>
         * Gets or sets the net amount for the current month.
         * </summary>
         */
        public decimal ThisMonthNet
        {
            get => _thisMonthNet;
            set
            {
                if (_thisMonthNet != value)
                {
                    _thisMonthNet = value;
                    OnPropertyChanged(nameof(ThisMonthNet));
                }
            }
        }
        /**
         * <summary>
         * Gets or sets the income for the current month.
         * </summary>
         */
        public decimal ThisMonthIncome
        {
            get => _thisMonthIncome;
            set
            {
                if (_thisMonthIncome != value)
                {
                    _thisMonthIncome = value;
                    OnPropertyChanged(nameof(ThisMonthIncome));
                }
            }
        }
        /**
         * <summary>
         * Gets or sets the expense for the current month.
         * </summary>
         */
        public decimal ThisMonthExpense
        {
            get => _thisMonthExpense;
            set
            {
                if (_thisMonthExpense != value)
                {
                    _thisMonthExpense = value;
                    OnPropertyChanged(nameof(ThisMonthExpense));
                }
            }
        }
        /**
         * <summary>
         * Gets or sets the total balance across all records.
         * </summary>
         */
        public decimal TotalBalance
        {
            get => _totalBalance;
            set
            {
                if (_totalBalance != value)
                {
                    _totalBalance = value;
                    OnPropertyChanged(nameof(TotalBalance));
                }
            }
        }
        /**
         * <summary>
         * Gets the amount of income transactions for the current month.
         * </summary>
         */
        public int ThisMonthIncomeTransactionAmount => MonthlyRecords.Where(mr => mr.Date.Month == DateTime.Now.Month && mr.Date.Year == DateTime.Now.Year).SelectMany(mr => mr.DailyRecords).SelectMany(dr => dr.Records).Count(r => r.Type == RecordType.Income);
        /**
         * <summary>
         * Gets the text representation of the income transactions for the current month.
         * </summary>
         */
        public string ThisMonthIncomeTransactionText => string.Format(Resources.Resources.thisMonthIncomeTransactions, ThisMonthIncomeTransactionAmount);
        /**
         * <summary>
         * Gets the amount of expense transactions for the current month.
         * </summary>
         */
        public int ThisMonthExpenseTransactionAmount => MonthlyRecords.Where(mr => mr.Date.Month == DateTime.Now.Month && mr.Date.Year == DateTime.Now.Year).SelectMany(mr => mr.DailyRecords).SelectMany(dr => dr.Records).Count(r => r.Type == RecordType.Expense);
        /**
         * <summary>
         * Gets the text representation of the expense transactions for the current month.
         * </summary>
         */
        public string ThisMonthExpenseTransactionText => string.Format(Resources.Resources.thisMonthExpenseTransactions, ThisMonthExpenseTransactionAmount);
        /**
         * <summary>
         * Gets the total number of transactions across all records.
         * </summary>
         */
        public int TotalTransactionAmount => MonthlyRecords.SelectMany(mr => mr.DailyRecords).SelectMany(mr => mr.Records).Count();
        /**
         * <summary>
         * Gets the text representation of the total transactions.
         * </summary>
         */
        public string TotalTransactionText => string.Format(Resources.Resources.TotalTransactions, TotalTransactionAmount);
        /**
         * <summary>
         * Indicates whether there are no records to display.
         * </summary>
         */
        public bool IsEmpty => MonthlyRecords.Count() == 0;
        /**
         * <summary>
         * Gets or sets the collection of monthly records.
         * </summary>
         */
        public ObservableCollection<MonthlyRecords> MonthlyRecords { get; set; }
        /**
         * <summary>
         * Command to switch between tabs in the records view.
         * </summary>
         */
        public ICommand SwitchTabCommand => new RelayCommand<string>((tab) =>
        {
            CultureInfo.CurrentCulture = new CultureInfo(CultureInfoHelper.Get());
            if (Enum.TryParse(tab, out RecordViewTabs selectedTab))
            {
                SelectedTab = selectedTab;
            }
        });
        /**
         * <summary>
         * Command to add a new record.
         * </summary>
         */
        public ICommand AddRecordCommand => new RelayCommand(() =>
        {
            _ = DatabaseService.AddRecordAsync(NewRecord.GetRecord());
            NewRecord = new RecordViewModel();
            SelectedTab = RecordViewTabs.Overview;
            LoadData();
            OnPropertyChanged(nameof(IsEmpty));
        });
        /**
         * <summary>
         * A view model for the records view, representing the a new record to add to the database.
         * </summary>
         */
        public RecordViewModel NewRecord
        {
            get => _newRecord;
            set
            {
                if (_newRecord != value)
                {
                    _newRecord = value;
                    OnPropertyChanged(nameof(NewRecord));
                }
            }
        }
        /**
         * <summary>
         * A view model for the records view, representing the record to edit.
         * </summary>
         */
        public RecordViewModel EditRecord
        {
            get => _editRecord;
            set
            {
                if (_editRecord != value)
                {
                    _editRecord = value;
                    OnPropertyChanged(nameof(EditRecord));
                }
            }
        }
        /**
         * <summary>
         * The currently selected tab in the records view.
         * </summary>
         */
        public RecordViewTabs SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged(nameof(SelectedTab));
                }
            }
        }
        /**
         * <summary>
         * The currently selected category for filtering records.
         * </summary>
         */
        public SelectableFilterCategories SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    LoadData();
                }
            }
        }
        public RecordsViewViewModel()
        {
            MonthlyRecords = new ObservableCollection<MonthlyRecords>();
            SelectedCategory = SelectableFilterCategories.All;
            LoadData();
            NewRecord = new RecordViewModel();
        }
        private async void LoadData()
        {
            List<Record> records;
            switch (SelectedCategory)
            {
                case SelectableFilterCategories.All:
                    records = await DatabaseService.GetRecordsAsync();
                    break;
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
                             Records = new ObservableCollection<RecordViewModel>(dr.ToList().Select(r => new RecordViewModel(r)).OrderBy(mr=>mr.Date))
                         })
                    )
                }).OrderBy(mr => mr.Date);
            foreach (var monthlyRecord in groupedRecords)
            {
                MonthlyRecords.Add(monthlyRecord);
            }
            ThisMonthNet = await DatabaseService.GetCurrentMonthNet();
            ThisMonthIncome = await DatabaseService.GetCurrentMonthIncome();
            ThisMonthExpense = await DatabaseService.GetCurrentMonthExpense();
            TotalBalance = await DatabaseService.GetTotalBalance();
            ReloadDisplay();
        }
        /**
         * <summary>
         * The command used to delete a record.
         * </summary>
         */
        public ICommand DeleteRecordCommand => new RelayCommand<RecordViewModel>((record) =>
        {
            if (NotificationService.AskConfirmation("Delete Record", "Are you sure you want to delete this record?"))
            {
                _ = DatabaseService.DeleteRecordAsync(record.Id);
                LoadData();
                OnPropertyChanged(nameof(IsEmpty));
                NotificationService.ShowNotification("Record Deleted", "The record has been successfully deleted.");
            }
        });
        /**
         * <summary>
         * The command used to edit a record.
         * </summary>
         */
        public ICommand EditRecordCommand => new RelayCommand<RecordViewModel>((record) =>
        {
            SelectedTab = RecordViewTabs.EditRecord;
            EditRecord = record;
        });
        /**
         * <summary>
         * The command used to submit the edited record.
         * </summary>
         */
        public ICommand EditRecordSubmitCommand => new RelayCommand(() =>
        {
            SelectedTab = RecordViewTabs.Overview;
            DatabaseService.EditRecordAsync(EditRecord.GetRecord()).Wait();
            LoadData();
            EditRecord = null;
        });
        /**
         * <summary>
         * The command used to navigate to the settings view.
         * </summary>
         */
        public ICommand NavigateToSettingsCommand => new RelayCommand(() =>
        {
            NavigationService.Navigate(new SettingsView());
        });
        /**
         * <summary>
         * Reloads the display of the records view.
         * </summary>
         */
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
