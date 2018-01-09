using System;
using Xamarin.Forms;

namespace TinyShopping.Converters
{
    public class FirstCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                return str.ToUpper()[0].ToString();
            }
            return "X";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FirstCharColorConverter : IValueConverter
    {
        private Color[] COLORS = new Color[] { Color.FromHex("#cdf1ff"), Color.FromHex("#b5c1ff"), Color.FromHex("#ffc1f3"), Color.FromHex("#ccf4b7"), Color.FromHex("#fffac7")};

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                var nr = (int)str.ToUpper()[0] % COLORS.Length;
                return COLORS[nr];
            }
            return Color.FromHex("#dddddd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
