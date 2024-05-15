using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Behaviors
{
    public class DateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                if (date.Date == today)
                {
                    return "Сегодня";
                }
                else if (date.Date == tomorrow)
                {
                    return "Завтра";
                }
                else
                {
                    return date.ToString("dd MMMM");
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

