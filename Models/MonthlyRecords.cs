using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace JuanNotTheHuman.Spending.Models
{
    /**
     * <summary>
     * A class representing monthly records of daily transactions.
     * </summary>
     */
    internal class MonthlyRecords
    {
        /**
         * <summary>
         * A property representing the date of the monthly records.
         * </summary>
        */
        public DateTime Date { get; set; }
        /**
         * <summary>
         * A collection of daily records for the month.
         * </summary>
         */
        public ObservableCollection<DailyRecords> DailyRecords { get; set; }
        /**
         * <summary>
         * Amount of income for the month.
         * </summary>
         */
        public decimal DailyRecordsIncome => DailyRecords.Sum(r => r.TotalIncome);
        /**
         * <summary>
         * Amount of expense for the month.
         * </summary>
         */
        public decimal DailyRecordsExpense => DailyRecords.Sum(r => r.TotalExpense);
        /**
         * <summary>
         * Net amount for the month, calculated as income minus expense.
         * </summary>
         */
        public decimal DailyRecordsNet => DailyRecordsIncome - DailyRecordsExpense;
        /**
         * <summary>
         * Total number of records for the month, calculated as the sum of all daily records.
         * </summary>
         */
        public int DailyRecordsCount => DailyRecords.Sum(r => r.RecordCount);
        /**
         * <summary>
         * Text representation of the total transactions for the month.
         * </summary>
         */
        public string DailyRecordsText => string.Format(Resources.Resources.TotalTransactions, DailyRecordsCount);
    }
}
