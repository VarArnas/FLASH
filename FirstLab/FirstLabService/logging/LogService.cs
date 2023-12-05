namespace FirstLabService.logging
{
    public class LogService : ILogService
    {
        public void LogTime()
        {
            string logFilePath = "logging/log.txt";

            File.AppendAllText(logFilePath, $"{DateTime.Now} - Log created ");
        }
    }
}
