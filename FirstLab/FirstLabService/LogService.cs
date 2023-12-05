namespace FirstLabService
{
    public class LogService : ILogService
    {
        public void LogTime()
        {
            string logFilePath = "log.txt";

            File.AppendAllText(logFilePath, $"{DateTime.Now} - Log created ");
        }
    }
}
