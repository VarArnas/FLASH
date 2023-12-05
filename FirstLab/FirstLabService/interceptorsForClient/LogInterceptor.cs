using Castle.DynamicProxy;

namespace FirstLabService.interceptorsForClient
{
    public class LogInterceptor : IInterceptor
    {
        private readonly string logFilePath = "logging/log.txt";

        public void Intercept(IInvocation invocation)
        {
            try
            {
                Log($"Time before interception in {invocation.Method.Name} at {DateTime.Now}");

                invocation.Proceed();
            }
            catch (Exception ex)
            {
                Log($"Exception in {invocation.Method.Name}: {ex.Message} at {DateTime.Now}");
                throw;
            }
        }

        private void Log(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(logMessage);
            }
        }
    }
}
