using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WorkTime.Models;

namespace WorkTime.Converters
{
    class TimeStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TimeStatus)value)
            {
                case TimeStatus.Err:
                    return new SolidColorBrush(Colors.Red);
                case TimeStatus.Ok:
                    return new SolidColorBrush(Colors.Black);
                default:
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
