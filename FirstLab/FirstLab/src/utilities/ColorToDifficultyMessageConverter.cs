using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FirstLab.src.utilities
{
    public class ColorToDifficultyMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string difficulty = "Medium";

            if (value is string colorString)
            {
                switch (colorString)
                {
                    case "Red":
                        difficulty = "Very easy";
                        break;

                    case "Green":
                        difficulty = "Easy";
                        break;

                    case "Yellow":
                        difficulty = "Medium";
                        break;

                    case "Blue":
                        difficulty = "Hard";
                        break;

                    case "Orange":
                        difficulty = "Very hard";
                        break;
                }
            }

            else if (value is SolidColorBrush colorBrush)
            {
                if (colorBrush.Color == Colors.Red)
                    difficulty = "Very easy";
                else if (colorBrush.Color == Colors.Green)
                    difficulty = "Easy";
                else if (colorBrush.Color == Colors.Yellow)
                    difficulty = "Medium";
                else if (colorBrush.Color == Colors.Blue)
                    difficulty = "Hard";
                else if (colorBrush.Color == Colors.Orange)
                    difficulty = "Very hard";
            }

            return difficulty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}