using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DateTimeApp.ViewModel
{
    public class NightThemeToggleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!bool.Parse(value.ToString()))
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
