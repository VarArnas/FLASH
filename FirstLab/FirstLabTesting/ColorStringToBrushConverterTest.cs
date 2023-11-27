using FirstLab.src.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FirstLabTesting
{
    public class ColorStringToBrushConverterTest
    {
        [Theory]
        [InlineData ("Red")]
        [InlineData ("Blue")]
        [InlineData ("CadetBlue")]
        [InlineData ("red")]

        public void Convert_PassingStringValuesOfColors_ReturnsColorOfPassedString(string input)
        {
            // Arange
            SolidColorBrush expectedResult = (SolidColorBrush) new BrushConverter().ConvertFrom(input)!;
            ColorStringToBrushConverter converter = new ColorStringToBrushConverter();

            // Act
            var result = (SolidColorBrush) converter.Convert(input);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(expectedResult.Color, result.Color);
        }

        [Theory]
        [InlineData ("word")]
        [InlineData ("")]
        [InlineData ("blues")]
        [InlineData ("234")]
        public void Convert_PassingIncorrectStringValues_ReturnsDefaultColor(string input)
        {
            // Arrange
            SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightBlue);
            ColorStringToBrushConverter converter = new ColorStringToBrushConverter();

            // Act
            var result = (SolidColorBrush)converter.Convert(input);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(defaultColor.Color, result.Color);
        }

        [Fact]
        public void Convert_PassingNonStringValues_ReturnsDefaultColor()
        {
            // Arrange
            SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightBlue);
            ColorStringToBrushConverter converter = new ColorStringToBrushConverter();
            int number = 7;
            char character = 'a';

            // Act
            var result1 = (SolidColorBrush)converter.Convert(number);
            var result2 = (SolidColorBrush)converter.Convert(character);

            // Assert
            Assert.IsType<SolidColorBrush>(result1);
            Assert.Equal(defaultColor.Color, result1.Color);
            Assert.IsType<SolidColorBrush>(result2);
            Assert.Equal(defaultColor.Color, result2.Color);
        }

        [Fact]
        public void Convert_HandlingNull_ReturnsDefaultColor()
        {
            // Arrange
            SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightBlue);
            ColorStringToBrushConverter converter = new ColorStringToBrushConverter();

            // Act
            var result = (SolidColorBrush)converter.Convert(null);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(defaultColor.Color, result.Color);
        }
    }
}
