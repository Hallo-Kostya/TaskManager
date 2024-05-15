using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Behaviors
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string colorString)
            {
                return Color.FromHex(colorString);
            }
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
