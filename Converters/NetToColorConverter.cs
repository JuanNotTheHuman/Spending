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
    public class NetToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal netValue)
            {
                if (netValue > 0)
                {
                    return Brushes.Green;
                }
                else if (netValue < 0)
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.Gray;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is SolidColorBrush color)
            {
                if(color.Color == Colors.Green)
                {
                    return 1m;
                }
                else if(color.Color == Colors.Red)
                {
                    return -1m;
                }
                else if(color.Color == Colors.Gray)
                {
                    return 0m;
                }
            }
            return 0m;
        }
    }
}
