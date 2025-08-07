using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Enumerables
{
    public enum RecordType
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "TypeIncome")]
        Income,
        [Display(ResourceType = typeof(Resources.Resources), Name = "TypeExpense")]
        Expense,
    }
}
