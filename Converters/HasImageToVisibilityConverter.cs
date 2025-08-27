using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JuanNotTheHuman.Spending.Converters
{
    internal class HasImageToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
            {
                if (value is byte[] bytes)
                {
                    return bytes.Length > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                }
                return System.Windows.Visibility.Collapsed;
            }
            else if ((string)parameter == "Reverse")
            {
                return value is byte[] bytes && bytes.Length > 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is System.Windows.Visibility visibility)
            {
                return visibility == System.Windows.Visibility.Visible ? new byte[1] : null;
            }
            return null;
        }
    }
}
