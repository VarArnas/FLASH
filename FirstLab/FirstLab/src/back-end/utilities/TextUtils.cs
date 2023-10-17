using System;
using System.IO;
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

        public static string ReturnDatabaseString()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Navigate up the directory tree to reach the desired directory
            while (!Directory.Exists(Path.Combine(currentDirectory, "src\\back-end\\database")))
            {
                // Go up one directory
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }

            return Path.Combine(currentDirectory, "src\\back-end\\database\\myDatabase.db");
        }
    }
}
