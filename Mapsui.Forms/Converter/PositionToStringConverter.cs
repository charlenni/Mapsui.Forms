using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mapsui.Forms.Converter
{
    public class PositionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter is string)
                return ((Xamarin.Forms.Maps.Position)value).ToString((string)parameter);

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
