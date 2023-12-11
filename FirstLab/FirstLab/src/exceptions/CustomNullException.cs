using System;
using System.IO;

namespace FirstLab.src.exceptions
{
    public class CustomNullException : Exception
    {
        public string defaultTime;

        public string defaultColor;

        public CustomNullException(string message) : base(message)
        {
            defaultTime = "System.Windows.Controls.ListBoxItem: 5 seconds";
            defaultColor = "Green";
        }

        public override string Message
        {
            get
            {
                return "Exception: " + base.Message;
            }
        }

        public static void LogException(Exception ex)
        {
            string logFilePath = "error.log";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"[Error] {DateTime.Now}: {ex.Message}");
                writer.WriteLine(ex.StackTrace);
                writer.WriteLine();
            }
        }
    }
}