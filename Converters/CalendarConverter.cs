using JuanNotTheHuman.Spending.Helpers;
using System;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a DateTime to a string representation based on the specified format.
     * </summary>
     */
    internal class CalendarConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a DateTime value to a string representation.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string format;
            if (parameter == null)
            {
                format = "D";
            }
            else
            {
                format = (string)parameter;
            }
            culture = new System.Globalization.CultureInfo(CultureInfoHelper.Get());
            if (value is DateTime dateTime)
            {
                return dateTime.ToString(format, culture);
            }
            return value;
        }
        /**
         * <summary>
         * Converts a string representation back to a DateTime value.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            culture = new System.Globalization.CultureInfo(CultureInfoHelper.Get());
            if (value is string dateString && DateTime.TryParse(dateString, culture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return value;
        }
    }
}
