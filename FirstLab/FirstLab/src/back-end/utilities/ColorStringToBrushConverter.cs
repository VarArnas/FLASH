using System;
using System.Windows.Data;
using System.Windows.Media;

namespace FirstLab.src.back_end.utilities;

public class ColorStringToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string colorString)
        {
            try
            {
                SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFrom(colorString)!;
                return brush;
            }
            catch (Exception)
            {
                return new SolidColorBrush(Colors.LightBlue);
            }
        }
        return new SolidColorBrush(Colors.LightBlue);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
