using System.Windows.Controls;

namespace FirstLab.src.back_end.utilities
{
    public static class ControllerUtils
    {
        public static void setDefaultText(TextBox textBox, string defaultText)
        {
            if (textBox.Text == "")
            {
                textBox.Text = defaultText;
            }

        }

        public static void setEmptyText(TextBox textBox, string defaultText)
        {
            if(textBox.Text == defaultText)
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
