namespace UserManagement.Services
{
   public class UsersService : IUsersService, IDisposable
   {
      private readonly ILoggerService _logger;

      public UsersService(ILoggerService logger)
      {
         _logger = logger;
      }

      public void Dispose()
      {
         var stop = "";
      }

      public bool SaveUsers(User user)
      {
         _logger.LogMessage("Ho scritto il log");

         return false;
      }
   }
}
