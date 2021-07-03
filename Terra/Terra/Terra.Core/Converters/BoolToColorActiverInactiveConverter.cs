using System;
using System.Globalization;
using Xamarin.Forms;

namespace Terra.Core.Converters
{
    public class BoolToColorActiverInactiveConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || (bool)value)
            {
                return Color.Transparent;
            }


            switch (parameter.ToString())
            {
                case "0" when (bool)value:
                    return Color.FromRgba(255, 255, 255, 0.6);
                case "1" when !(bool)value:// when value is false it will return color code, otherwise it will return default case
                    return Color.FromHex("#95d9d9d9");
                case "2" when (bool)value:
                    return Color.FromHex("#FF4A4A");
                case "2":
                    return Color.FromHex("#ced2d9");
                case "3" when (bool)value:
                    return Color.FromHex("#959eac");
                case "3":
                    return Color.FromHex("#ced2d9");
                default:
                    return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
