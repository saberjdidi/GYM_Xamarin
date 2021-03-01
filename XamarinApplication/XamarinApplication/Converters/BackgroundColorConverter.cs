using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string && value != null)
            {
                string s = (string)value;
                switch (s)
                {
                    case "ON":
                        return Color.FromHex("#04D167");
                    case "ST":
                        return Color.FromHex("#F7B140");
                    case "OF":
                        return Color.FromHex("#F42C04");
                    default:
                        return Color.FromHex("#F3CA40");
                }

            }
            return Color.FromHex("#F3CA40");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
