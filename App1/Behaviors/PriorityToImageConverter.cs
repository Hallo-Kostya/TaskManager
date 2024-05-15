using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.Behaviors
{
    public class PriorityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EnumPriority priority)
            {
                switch (priority)
                {
                    case EnumPriority.Нет:
                        return "without1.png";
                    case EnumPriority.Высокий:
                        return "high1.png";
                    case EnumPriority.Средний:
                        return "medium1.png";
                    case EnumPriority.Низкий:
                        return "low1.png";
                    default:
                        return "without1.png";
                }
            }
            return "without1.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
