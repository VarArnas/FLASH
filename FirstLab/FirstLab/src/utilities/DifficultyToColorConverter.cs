using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FirstLab.src.utilities
{
    public  class DifficultyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string colorString = "Black";
            if (value is string difficulty)
            {
                switch (difficulty)
                {
                    case "Very easy":
                        colorString = "IndianRed";
                        break;

                    case "Easy":
                        colorString = "Green";
                        break;

                    case "Medium":
                        colorString = "Yellow";
                        break;

                    case "Hard":
                        colorString = "RoyalBlue";
                        break;

                    case "Very hard":
                        colorString = "Orange";
                        break;
                }
            }

            SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFrom(colorString)!;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
