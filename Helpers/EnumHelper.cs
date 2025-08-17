using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Linq;
namespace JuanNotTheHuman.Spending.Helpers
{
    /**
     * <summary>
     * A helper class for working with enumerations.
     * </summary>
     */
    internal static class EnumHelper
    {
        /**
         * <summary>
         * An array of all the categories defined in the Category enumeration.
         * </summary>
         */
        public static Array Categories => Enum.GetValues(typeof(Category));
        public static string GetDisplayName(this Enum enumvalue){
            var memberInfo = enumvalue.GetType().GetMember(enumvalue.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var attributes = memberInfo.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((System.ComponentModel.DisplayNameAttribute)attributes[0]).DisplayName;
                }
            }
            return enumvalue.ToString();
        }
    }
}
