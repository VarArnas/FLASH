using System.Windows.Controls;

namespace FirstLab.src.back_end.utilities
{
    internal class TextUtils
    {
        public static void SetDefaultText(TextBox textBox, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = defaultText;
            }

        }

        public static void SetEmptyText(TextBox textBox, string defaultText)
        {
            if (textBox.Text == defaultText)
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
