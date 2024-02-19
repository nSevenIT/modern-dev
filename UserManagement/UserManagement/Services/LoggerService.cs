namespace UserManagement.Services
{
   public class LoggerService : ILoggerService // IDisposable
   {
      public void LogMessage(string message)
      {
         File.WriteAllText("log.log", message);
      }

      public void LogError(Exception exception, string? message = "")
      {
         File.WriteAllText("error.log", exception.Message + "\n" + message);
      }
   }
}
