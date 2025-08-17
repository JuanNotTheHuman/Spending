using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Helpers;
using JuanNotTheHuman.Spending.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    internal class EnumToLocalizedConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts an enum value to its localized string representation.
         * </summary>
         * <param name="value">The enum value to convert.</param>
         * <param name="targetType">The type of the target property.</param>
         * <param name="parameter">An optional parameter for additional context.</param>
         * <param name="culture">The culture to use for localization.</param>
         * <returns>The localized string representation of the enum value.</returns>
         */
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                return LocalizationService.Instance[enumValue.GetDisplayName()];
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is string strValue && Enum.IsDefined(targetType, strValue))
            {
                return Enum.Parse(targetType, strValue);
            }
            return null;
        }
    }
}
