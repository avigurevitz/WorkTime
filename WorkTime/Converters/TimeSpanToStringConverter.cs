using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorkTime.Models;

namespace WorkTime.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        private static TimeSpan MIN_WORK_TIME = new TimeSpan(-3, -30, 0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var overTime = (TimeSpan) value;
            if (overTime > TimeSpan.Zero)
                return TimeStatus.Over.ToString();
            else if (overTime > MIN_WORK_TIME)
                return TimeStatus.Ok.ToString();
            else
                return TimeStatus.Err.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
