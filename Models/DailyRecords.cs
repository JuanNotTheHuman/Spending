using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JuanNotTheHuman.Spending.Models
{
    /**
     * <summary>
     * Represents a collection of records for a specific day.
     * </summary>
     */
    internal class DailyRecords
    {
        /**
         * <summary>
         * Represents the date for which the records are collected.
         * </summary>
         */
        public DateTime Date { get; set; }
        /**
         * <summary>
         * Represents the collection of records for the specified date.
         * </summary>
         */
        public ObservableCollection<RecordViewModel> Records { get; set; }
        /**
         * <summary>
         * Total income for the day, calculated by summing amounts of all income records.
         * </summary>
         */
        public decimal TotalIncome => Records.Where(r => r.Type == RecordType.Income).Sum(r => r.Amount);
        /**
         * <summary>
         * Total expenses for the day, calculated by summing amounts of all expense records.
         * </summary>
         */
        public decimal TotalExpense => Records.Where(r => r.Type == RecordType.Expense).Sum(r => r.Amount);
        /**
         * <summary>
         * Net amount for the day, calculated as total income minus total expenses.
         * </summary>
         */
        public decimal Net => TotalIncome - TotalExpense;
        /**
         * <summary>
         * The total number of records for the day.
         * </summary>
         */
        public int RecordCount => Records.Count();
    }
}
