using Spending.Enumerables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Spending.Converters
{
    internal class NewRecordSelectedTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is RecordType recordType)
            {
                if (parameter is string param && Enum.TryParse(param, out RecordType selectedType))
                {
                    return recordType == selectedType;
                }
            }
            return false;
        }

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
