using Castle.DynamicProxy;
using System;
using System.IO;

namespace FirstLabService
{
    public class TempInterceptor : IInterceptor
    {
        private readonly string logFilePath = "log.txt";

        public void Intercept(IInvocation invocation)
        {
            try
            {
                Log($"Before {invocation.Method.Name} at {DateTime.Now}");

                invocation.Proceed();

                Log($"After {invocation.Method.Name} at {DateTime.Now}");
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
