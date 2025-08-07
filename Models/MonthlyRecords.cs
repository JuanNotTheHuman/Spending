using System;
using System.Collections.ObjectModel;
using System.Linq;
using Spending.Resources;
namespace Spending.Models
{
    internal class MonthlyRecords
    {
        public DateTime Date { get; set; }
        public ObservableCollection<DailyRecords> DailyRecords { get; set; }
        public decimal DailyRecordsIncome => DailyRecords.Sum(r => r.TotalIncome);
        public decimal DailyRecordsExpense => DailyRecords.Sum(r => r.TotalExpense);
        public decimal DailyRecordsNet => DailyRecordsIncome - DailyRecordsExpense;
        public int DailyRecordsCount => DailyRecords.Sum(r => r.RecordCount);
        public string DailyRecordsText => string.Format(Resources.Resources.TotalTransactions, DailyRecordsCount);
    }
}
