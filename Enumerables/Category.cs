using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Enumerables
{
    public enum Category
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
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryIncome")]
        Income,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryPets")]
        Pets,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryVet")]
        Vet,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryGifts")]
        Gifts,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryTravel")]
        Travel,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryHome")]
        Home,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategorySubscriptions")]
        Subscriptions,
        [Display(ResourceType = typeof(Resources.Resources), Name = "CategoryPhone")]
        Phone,
    }
}
