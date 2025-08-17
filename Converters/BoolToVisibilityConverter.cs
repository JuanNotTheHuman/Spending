using System;
using System.Globalization;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a boolean value to a visibility value.
     * </summary>
     */
    internal class BoolToVisibilityConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a boolean value to a visibility value.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }
        /**
         * <summary>
         * Converts a visibility value back to a boolean value.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Visibility visibility)
            {
                return visibility == System.Windows.Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }
}
