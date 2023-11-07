using FirstLab.src.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("AaBbCcDd", "ABCD")]
        [InlineData("Cat", "C")]
        
        [InlineData(null, "")]
        public void ExtractCapLetters_PassingWordsWithCapitalLetters_ReturnsOnlyCapitalLetters(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.ExtractCapLetters(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData("word", "")]
        [InlineData("", "")]
        public void ExtractCapLetters_PassingWordsWithoutCapitalLetters_ReturnsEmptyString(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.ExtractCapLetters(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ExtractCapLetters_HandlingNull_ReturnsEmptyString()
        {
            // Arrange
            string? input = null;
            
            // Act
            var result = StringExtensions.ExtractCapLetters(input);

            // Assert
            Assert.True(result.Equals(""));
        }


        [Theory]
        [InlineData("word", "WORD")]
        [InlineData("Cat", "CAT")]
        [InlineData("CAPS", "CAPS")]
        [InlineData("", "")]
        public void Capitalize_PassingWordsWithOnlyLetters_ReturnsAllCapitalLetters(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.Capitalize(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData("4fff", "4FFF")]
        [InlineData("%#dfff", "%#DFFF")]

        public void Capitalize_PassingWordsWithLettersAndSymbols_ReturnsCapitalizedLetters(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.Capitalize(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void Capitalize_HandlingNull_ReturnsEmptyString()
        {
            // Arrange
            string? input = null;

            // Act
            var result = StringExtensions.Capitalize(input);

            // Assert
            Assert.True(result.Equals(""));
        }


        [Theory]
        [InlineData("Word")]
        [InlineData("A word")]
        [InlineData("4")]
        [InlineData("")]
        public void ContainsSymbols_PassingWordsWithoutSymbols_ReturnsFalse(string input)
        {
            // Arrange
            // Act
            var result = StringExtensions.ContainsSymbols(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ContainsSymbols_PassingWordsWithSymbols_ReturnsTrue()
        {
            // Arrange
            string input = "%#$$Aa";

            // Act
            var result = StringExtensions.ContainsSymbols(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ContainsSymbols_HandlingNull_ReturnsFalse()
        {
            // Arrange
            string? input = null;

            // Act
            var result = StringExtensions.ContainsSymbols(input);

            // Assert
            Assert.False(result);
        }
    }
}
