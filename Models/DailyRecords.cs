using Spending.Enumerables;
using Spending.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Models
{
    internal class DailyRecords
    {
        public DateTime Date { get; set; }
        public ObservableCollection<RecordViewModel> Records { get; set; }
        public decimal TotalIncome => Records.Where(r => r.Type == RecordType.Income).Sum(r => r.Amount);
        public decimal TotalExpense => Records.Where(r => r.Type == RecordType.Expense).Sum(r => r.Amount);
        public decimal Net => TotalIncome - TotalExpense;
        public int RecordCount => Records.Count();
    }
}
