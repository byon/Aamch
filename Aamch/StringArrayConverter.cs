using System;
using System.Windows.Data;

namespace Aamch
{
    [ValueConversion(typeof(string[]), typeof(string))]
    class StringArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            return string.Join(", ", (string[])value);
        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return ((string)value).Split(',');
        }
    }
}
