using Spending.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Spending.Converters
{
    internal class CalendarConverter : IValueConverter
    {
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
            culture = new System.Globalization.CultureInfo(ApplicationCultureInfoHelper.Get());
            if (value is DateTime dateTime)
            {
                return dateTime.ToString(format, culture);
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            culture = new System.Globalization.CultureInfo(ApplicationCultureInfoHelper.Get());
            if (value is string dateString && DateTime.TryParse(dateString, culture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return value;
        }
    }
}
