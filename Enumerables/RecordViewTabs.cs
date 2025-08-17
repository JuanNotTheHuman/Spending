using System.ComponentModel.DataAnnotations;

namespace JuanNotTheHuman.Spending.Enumerables
{
    /**
     * <summary>
     * An Enumeration representing the tabs available in the record view.
     * </summary>
     */
    internal enum RecordViewTabs
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_Overview")]
        Overview,
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_AddRecords")]
        AddRecord,
        [Display(ResourceType = typeof(Resources.Resources), Name = "RecordViewTab_EditRecords")]
        EditRecord,
    }
}
