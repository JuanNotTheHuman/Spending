using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a RecordType to an icon representation.
     * </summary>
     */
    internal class RecordTypeIconConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a RecordType to an icon string.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordType recordType)
            {
                if (recordType == RecordType.Income)
                {
                    return "🔺";
                }
                else if (recordType == RecordType.Expense)
                {
                    return "🔻";
                }
            }
            return "?";
        }
        /**
         * <summary>
         * Converts an icon string back to a RecordType.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string icon)
            {
                switch (icon)
                {
                    case "🔺":
                        return RecordType.Income;
                    case "🔻":
                        return RecordType.Expense;
                }
            }
            return RecordType.Expense;
        }
    }
}
