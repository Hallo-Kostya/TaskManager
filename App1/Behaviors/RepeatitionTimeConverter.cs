using Plugin.InputKit.Shared.Validations;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace App1.Behaviors
{
    public class RepeatitionTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int multiplier)
            {
                if (multiplier == 0)
                    return "Нет";
                else if (multiplier == 1)
                    return "Каждый день";
                else if (multiplier == 3)
                    return "Каждые 3 дня";
                else if (multiplier == 7)
                    return "Каждую неделю";
                else
                {
                    if ((multiplier / 10 != 1) && (multiplier % 10 == 2 || multiplier % 10 == 3 || multiplier % 10 == 4))
                    {
                        return "Раз в " + multiplier.ToString() + " дня";
                    }
                    else if (multiplier / 10 == 1 || multiplier % 10 == 0 || multiplier%10==5 || multiplier % 10 == 6 || multiplier % 10 == 7 || multiplier % 10 == 8 || multiplier % 10 == 9)
                    {
                        return "Раз в " + multiplier.ToString() + " дней";
                    }
                    else
                    {
                        return "Раз в " + multiplier.ToString() + " день";
                    }

                } 

            }
            return "Нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
