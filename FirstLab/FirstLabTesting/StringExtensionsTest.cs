using FirstLab.src.back_end.utilities;
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
        [InlineData("word", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void StringExtensions_ExtractCapLetters_ReturnsString(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.ExtractCapLetters(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData("word", "WORD")]
        [InlineData("Cat", "CAT")]
        [InlineData("CAPS", "CAPS")]
        [InlineData("4fff", "4FFF")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void StringExtensions_Capitalize_ReturnsString(string input, string expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.Capitalize(input);

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData("Word", false)]
        [InlineData("A word", false)]
        [InlineData("%#$$Aa", true)]
        [InlineData("4", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void StringExtensions_ContainsSymbols_ReturnsBoolean(string input, bool expected)
        {
            // Arrange
            // Act
            var result = StringExtensions.ContainsSymbols(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
