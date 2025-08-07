using Spending.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Spending.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            culture = new CultureInfo(ApplicationCultureInfoHelper.Get());
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C", culture);
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter,CultureInfo culture)
        {
            culture = new CultureInfo(ApplicationCultureInfoHelper.Get());
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, culture, out var result))
            {
                return result;
            }
            return value;
        }
    }
}
