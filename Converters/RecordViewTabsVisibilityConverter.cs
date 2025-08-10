using JuanNotTheHuman.Spending.Enumerables;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JuanNotTheHuman.Spending.Converters
{
    /**
     * <summary>
     * Converts a RecordViewTabs enum value to a visibility value based on the selected tab.
     * </summary>
     */
    internal class RecordViewTabsVisibilityConverter : IValueConverter
    {
        /**
         * <summary>
         * Converts a RecordViewTabs enum value to a visibility value.
         * </summary>
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordViewTabs selectedTab && parameter is string tabName && Enum.TryParse(tabName, out RecordViewTabs thisTab))
            {
                return selectedTab == thisTab ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            return System.Windows.Visibility.Collapsed;
        }
        /**
         * <summary>
         * Converts a visibility value back to a RecordViewTabs enum value.
         * </summary>
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Visibility visibility && parameter is string tabName && Enum.TryParse(tabName, out RecordViewTabs thisTab))
            {
                return visibility == System.Windows.Visibility.Visible ? thisTab : Binding.DoNothing;
            }
            return Binding.DoNothing;
        }
    }
}
