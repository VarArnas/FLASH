using System.IO;
using System.Windows.Controls;

namespace FirstLab.src.back_end.utilities;

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
        string baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        string projectDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\bin"));
        string databasePath = Path.Combine(projectDirectory, "src\\back-end\\data\\myDatabase.db");
        return databasePath;
    }
}
