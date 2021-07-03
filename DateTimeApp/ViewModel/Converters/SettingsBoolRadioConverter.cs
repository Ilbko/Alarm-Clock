using System;
using System.Globalization;
using System.Windows.Data;

namespace DateTimeApp.ViewModel
{
    public class SettingsBoolRadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()) == bool.Parse(parameter.ToString()))
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bool.Parse(parameter.ToString());
        }
    }
}
