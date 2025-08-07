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
    internal class RecordViewTabsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is RecordViewTabs selectedTab && parameter is string tabName &&  Enum.TryParse(tabName, out RecordViewTabs thisTab)){
                return selectedTab == thisTab ? Brushes.White : Brushes.LightGray;
            }
            return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
