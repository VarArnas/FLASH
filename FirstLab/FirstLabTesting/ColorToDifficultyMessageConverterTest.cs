using FirstLab.src.utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FirstLabTesting
{
    public class ColorToDifficultyMessageConverterTest
    {
        [Theory]
        [InlineData ("IndianRed", "Very easy")]
        [InlineData ("Pink", "Easy")]
        [InlineData ("Yellow", "Medium")]
        [InlineData ("RoyalBlue", "Hard")]
        [InlineData ("Orange", "Very hard")]
        public void Convert_PassingStringValuesOfAvailableColors_ReturnsAppropriateDifficultyMessageString(string input, string expectedResult)
        {
            // Arrange
            ColorToDifficultyMessageConverter converter = new ColorToDifficultyMessageConverter();

            // Act
            var result = (string) converter.Convert(input);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData ("one")]
        [InlineData ("one two")]
        [InlineData("Green")]
        [InlineData ("Grey")]
        [InlineData ("")]
        public void Convert_PassingIncorrectStringValues_ReturnsDefaultDifficulty(string input)
        {
            // Arrange
            ColorToDifficultyMessageConverter converter = new ColorToDifficultyMessageConverter();
            string defaultMessage = "Medium";

            // Act
            var result = (string)converter.Convert(input);

            // Assert
            Assert.Equal(defaultMessage, result);
        }

        [Fact]
        public void Convert_PassingNonStringValues_ReturnsDefaultDifficulty()
        {
            // Arrange
            string defaultMessage = "Medium";
            ColorToDifficultyMessageConverter converter = new ColorToDifficultyMessageConverter();
            int number = 7;
            char character = 'a';

            // Act
            var result1 = (string)converter.Convert(number);
            var result2 = (string)converter.Convert(character);

            // Assert
            Assert.Equal(defaultMessage, result1);
            Assert.Equal(defaultMessage, result2);
        }

        [Fact] 
        public void Convert_HandlingNull_ReturnsDefaultDifficulty()
        {
            // Arrange
            ColorToDifficultyMessageConverter converter = new ColorToDifficultyMessageConverter();
            string defaultMessage = "Medium";

            // Act
            var result = (string)converter.Convert(null);

            // Assert
            Assert.Equal(defaultMessage, result);
        }
    }
}
