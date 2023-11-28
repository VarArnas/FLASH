using FirstLab.src.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FirstLabTesting
{
    public class DifficultyToColorConverterTest
    {
        [Theory]
        [InlineData ("Very easy", "Green")]
        [InlineData ("Easy", "Green")]
        [InlineData ("Medium", "Yellow")]
        [InlineData ("Hard", "Red")]
        [InlineData ("Very hard", "Red")]
        public void Convert_PassingCorrectStringDifficultyMessages_ReturnsNonBlackColor(string input, string expectedResult)
        {
            // Arrange
            DifficultyToColorConverter converter = new DifficultyToColorConverter();
            SolidColorBrush expectedColor = (SolidColorBrush)new BrushConverter().ConvertFrom(expectedResult)!;

            // Act
            var result = (SolidColorBrush) converter.Convert(input);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(expectedColor.Color, result.Color);
        }

        [Theory]
        [InlineData ("Normal")]
        [InlineData ("two words")]
        [InlineData ("")]
        public void Convert_PassingIncorrectStringValues_ReturnsDefaultColor(string input)
        {
            // Arrange
            string defaultColor = "Black";
            DifficultyToColorConverter converter = new DifficultyToColorConverter();
            SolidColorBrush expectedColor = (SolidColorBrush)new BrushConverter().ConvertFrom(defaultColor)!;

            // Act
            var result = (SolidColorBrush)converter.Convert(input);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(expectedColor.Color, result.Color);
        }

        [Fact]
        public void Convert_PassingNonStringValues_ReturnsDefaultColor()
        {
            // Arrange
            string defaultColor = "Black";
            DifficultyToColorConverter converter = new DifficultyToColorConverter();
            SolidColorBrush expectedColor = (SolidColorBrush)new BrushConverter().ConvertFrom(defaultColor)!;
            int number = 7;
            char character = 'a';

            // Act
            var result1 = (SolidColorBrush) converter.Convert(number);
            var result2 = (SolidColorBrush) converter.Convert(character);

            // Assert
            Assert.IsType<SolidColorBrush>(result1);
            Assert.IsType<SolidColorBrush>(result2);
            Assert.Equal(expectedColor.Color, result1.Color);
            Assert.Equal(expectedColor.Color, result2.Color);
        }

        [Fact]
        public void Convert_HandlingNull_ReturnsDefaultColor()
        {
            // Arrange
            string defaultColor = "Black";
            DifficultyToColorConverter converter = new DifficultyToColorConverter();
            SolidColorBrush expectedColor = (SolidColorBrush)new BrushConverter().ConvertFrom(defaultColor)!;
            
            // Act
            var result = (SolidColorBrush) converter.Convert(null);

            // Assert
            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(expectedColor.Color, result.Color);
        }
    }
}
