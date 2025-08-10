using JuanNotTheHuman.Spending.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a decimal value to a currency string and vice versa.
     * </summary>
     */
    internal class CurrencyConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a decimal value to a currency string.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            culture = new CultureInfo(CultureInfoHelper.Get());
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C", culture);
            }
            return value;
        }
        /**
         * <summary>
         * Converts a currency string back to a decimal value.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter,CultureInfo culture)
        {
            culture = new CultureInfo(CultureInfoHelper.Get());
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, culture, out var result))
            {
                return result;
            }
            return value;
        }
    }
}
