using System;
using System.Globalization;
using System.Windows.Data;

namespace MemoSoftV3.Models.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new DateTime(0).ToString(CultureInfo.InvariantCulture);
            }

            var dateTime = (DateTime)value;
            return dateTime.ToString("yyyy/MM/dd HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}