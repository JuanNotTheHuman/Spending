using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a RecordType to a corresponding color.
     * </summary>
     */
    internal class RecordTypeColorConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a RecordType to a SolidColorBrush.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordType recordType)
            {
                if (recordType == RecordType.Income)
                {
                    return Brushes.Green;
                }
                else if (recordType == RecordType.Expense)
                {
                    return Brushes.Red;
                }
            }
            return Brushes.Gray;
        }
        /**
         * <summary>
         * Converts a SolidColorBrush back to a RecordType.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                if (brush.Color == Colors.Green)
                {
                    return RecordType.Income;
                }
                else if (brush.Color == Colors.Red)
                {
                    return RecordType.Expense;
                }
            }
            return Binding.DoNothing;
        }
    }
}
