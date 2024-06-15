using System;
using System.Globalization;
using Xamarin.Forms;

namespace App1.Behaviors
{
    public class NotificationTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int multiplier)
            {
                switch (multiplier)
                {
                    case 0:
                        return "Во время";
                    case -5:
                        return "За 5 минут";
                    case -30:
                        return "За 30 минут";
                    case -60:
                        return "За 1 час";
                    case -1440:
                        return "За 1 день";
                    case 1:
                        return "Нет";
                    case 2:
                        return "Произвольно";
                }
            }
            return "Произвольно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
