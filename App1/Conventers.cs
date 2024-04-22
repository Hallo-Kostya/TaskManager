using System;
using Xamarin.Forms;

namespace App1.Converters
{
    public class PriorityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is string priority))
                return null;

            switch (priority.ToLower())
            {
                case "без приоритета":
                    return "eye.png";
                case "низкий":
                    return "user.png";
                case "средний":
                    return "trash.png";
                case "высокий":
                    return "phone.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

