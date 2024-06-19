using Plugin.InputKit.Shared.Validations;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace App1.Behaviors
{
    public class CleaningTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int multiplier)
            {
                if (multiplier == 24)
                    return "Произвольно";
                else if (multiplier == 72)
                    return "Раз в 3 дня";
                else if (multiplier == 168)
                    return "Раз в неделю";
                else if (multiplier == 720)
                    return "Раз в 30 дней";
                else
                {
                    var muti = multiplier / 24;
                    if ((muti / 10 != 1) && (muti % 10 == 2 || muti % 10 == 3 || muti % 10 == 4))
                    {
                        return "Раз в " + muti.ToString() + " дня";
                    }
                    else if (muti / 10 == 1 || muti % 10 == 0 || muti % 10 == 5 || muti % 10 == 6 || muti % 10 == 7 || muti % 10 == 8 || muti % 10 == 9)
                    {
                        return "Раз в " + muti.ToString() + " дней";
                    }
                    else
                    {
                        return "Раз в " + muti.ToString() + " день";
                    }

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
