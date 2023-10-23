using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FirstLab.src.back_end.utilities;

namespace FirstLabTesting
{
    public class TextUtilsTest
    {
        [Theory]
        [InlineData("", "word", "word")]
        [InlineData("  ", "word", "word")]
        [InlineData(null, "word", "word")]
        [InlineData("Not empty", "word", "Not empty")]
        public void TextUtils_SetDefaultText_ReturnsVoid(string textBoxString, string input, string expected)
        {
            // Arrange
            TextBox textBox = new TextBox();
            textBox.Text = textBoxString;

            // Act
            TextUtils.SetDefaultText(textBox, input);
            var result = textBox.Text;

            // Assert
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData("", "word", "")]
        [InlineData("word", "", "word")]
        [InlineData(null, "word", null)]
        [InlineData("word", null, "word")]
        [InlineData("word", "word", "word")]
        public void TextUtils_SetEmptyText_ReturnsVoid(string textBoxString, string input, string expected)
        {
            // Arrange
            TextBox textBox = new TextBox();
            textBox.Text = textBoxString;

            // Act
            TextUtils.SetEmptyText(textBox, input);
            var result = textBox.Text;

            // Assert
            Assert.True(result.Equals(expected));
        }
    }
}
