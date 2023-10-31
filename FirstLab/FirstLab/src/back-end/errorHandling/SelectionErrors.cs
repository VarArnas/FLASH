using System;
using System.IO;

namespace FirstLab.src.back_end.errorHandling
{
    public class SelectionErrors : Exception
    {

        public string defaultTime;

        public string defaultColor;

        public SelectionErrors()
        {
        }

        public SelectionErrors(string message) : base(message)
        {
            defaultTime = "System.Windows.Controls.ListBoxItem: 5 seconds";
            defaultColor = "System.Windows.Controls.ListBoxItem: Green";
        }

        public SelectionErrors(string message, Exception innerException) : base(message, innerException)
        {
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