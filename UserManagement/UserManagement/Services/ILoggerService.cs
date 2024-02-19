namespace UserManagement.Services
{
   public interface ILoggerService
   {
      void LogMessage(string message);

      void LogError(Exception exception, string? message = "");

   }
}
