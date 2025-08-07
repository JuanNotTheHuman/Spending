using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Enumerables
{
    public enum RecordViewTabs
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_Overview")]
        Overview,
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_AddRecords")]
        AddRecord,
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_EditRecords")]
        EditRecord,
    }
}
