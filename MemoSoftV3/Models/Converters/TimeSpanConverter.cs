using System;
using System.Globalization;
using System.Windows.Data;

namespace MemoSoftV3.Models.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? TimeSpan.Zero
                : TimeSpan.FromSeconds(Math.Floor(((TimeSpan)value).TotalSeconds));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}