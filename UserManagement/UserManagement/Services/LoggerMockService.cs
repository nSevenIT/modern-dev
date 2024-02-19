
namespace UserManagement.Services
{
   public class LoggerMockService : ILoggerService
   {
      public void LogError(Exception exception, string? message = "")
      {
         return;
      }

      public void LogMessage(string message)
      {
         return;
      }
   }
}

