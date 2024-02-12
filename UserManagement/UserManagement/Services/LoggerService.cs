namespace UserManagement.Services
{
   public class LoggerService : ILoggerService, IDisposable
   {
      public void LogMessage(string message)
      {
         File.WriteAllText("log.log", message);
      }

      public void Dispose()
      {
         var stop = "stop";
      }

   }
}
