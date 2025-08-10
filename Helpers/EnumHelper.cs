using System;
using JuanNotTheHuman.Spending.Enumerables;
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
    }
}
