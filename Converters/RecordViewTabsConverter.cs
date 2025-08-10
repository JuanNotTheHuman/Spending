using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a RecordViewTabs enum value to a Brush based on the selected tab.
     * Used in the RecordView to highlight the currently selected tab.
     * </summary>
     */
    internal class RecordViewTabsConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a RecordViewTabs enum value to a Brush.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is RecordViewTabs selectedTab && parameter is string tabName &&  Enum.TryParse(tabName, out RecordViewTabs thisTab)){
                return selectedTab == thisTab ? Brushes.White : Brushes.LightGray;
            }
            return Brushes.LightGray;
        }
        /**
         * <summary>
         * Converts a Brush back to a RecordViewTabs enum value.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Brush brush && parameter is string tabName && Enum.TryParse(tabName, out RecordViewTabs thisTab))
            {
                return brush == Brushes.White ? thisTab : Binding.DoNothing;
            }
            return Binding.DoNothing;
        }
    }
}
