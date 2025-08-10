using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanNotTheHuman.Spending.Enumerables
{
    /**
     * <summary>
     * An Enumeration representing the type of financial record.
     * </summary>
     */
    internal enum RecordType
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "TypeIncome")]
        Income,
        [Display(ResourceType = typeof(Resources.Resources), Name = "TypeExpense")]
        Expense,
    }
}
