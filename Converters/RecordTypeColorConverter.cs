using Spending.Enumerables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Spending.Converters
{
    internal class RecordTypeColorConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is SolidColorBrush brush)
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
