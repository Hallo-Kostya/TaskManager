using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App1.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (targetType == typeof(Color))
                {
                    return boolValue ? Color.FromHex("#90000000") : Color.Black;
                }

                if (targetType == typeof(ImageSource))
                {
                    return boolValue ? "clock1.png" : "time.png";
                }
                if (targetType == typeof(bool))
                {
                    return boolValue ? false : true;
                }
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (targetType == typeof(Color))
                {
                    return boolValue ? Color.Gray : Color.AliceBlue;
                }
            }
            return Color.AliceBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToTagColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (targetType == typeof(Color))
                {
                    return boolValue ? Color.FromHex("#3C3C3C") : Color.FromHex("#952EAF");
                }
            }
            return Color.FromHex("#952EAF");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            return string.IsNullOrEmpty(text) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemCountToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int itemCount)
            {
                int baseHeight = 55;
                int additionalHeight = 84 * itemCount;

                int calculatedHeight = baseHeight + additionalHeight;

                return calculatedHeight;
            }

            return 55;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DoneItemCountToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int itemCount)
            {
                int calculatedHeight = 83 * itemCount;

                return calculatedHeight;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
