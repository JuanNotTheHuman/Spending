using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a RecordType to a boolean indicating whether it matches the selected type.
     * </summary>
     */
    internal class NewRecordSelectedTypeConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a RecordType to a boolean indicating whether it matches the selected type.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordType recordType)
            {
                if (parameter is string param && Enum.TryParse(param, out RecordType selectedType))
                {
                    return recordType == selectedType;
                }
            }
            return false;
        }
        /**
         * <summary>
         * Converts a boolean indicating whether the record type is selected back to a RecordType.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected && parameter is string param)
            {
                if (Enum.TryParse(param, out RecordType selectedType))
                {
                    return selectedType;
                }
            }
            return Binding.DoNothing;
        }
    }
}
