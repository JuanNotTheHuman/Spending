using System.ComponentModel.DataAnnotations;

namespace Spending.Enumerables
{
    public enum SelectableFilterCategories
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryFood")]
        Food,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryTransport")]
        Transport,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryEntertainment")]
        Entertainment,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryHealth")]
        Health,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryEducation")]
        Education,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryShopping")]
        Shopping,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryUtilities")]
        Utilities,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryOther")]
        Other,
        [Display(ResourceType = typeof(Resources.Resources), Name = "SelectableFilterCategory_Incomes")]
        Incomes,
        [Display(ResourceType = typeof(Resources.Resources), Name = "SelectableFilterCategory_Expenses")]
        Expenses,
        [Display(ResourceType = typeof(Resources.Resources), Name = "SelectableFilterCategory_All")]
        All
    }
}
