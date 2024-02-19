using Microsoft.Extensions.Options;
using UserManagement.Options;

namespace UserManagement.Services
{
   public class UsersService : IUsersService // , IDisposable
   {
      private readonly ApplicationOptions _options;
      private readonly ILoggerService _logger;

      public UsersService(IOptions<ApplicationOptions> options, ILoggerService logger)
      {
         _options = options!.Value;
         _logger = logger;
      }

      public List<User> GetUsers(int? pageSize = 0)
      {
         List<User> user = [
            new User(1),
            new User(2)
         ];

         return user.Skip(1).Take(pageSize ?? _options.DefaultPageSize).ToList();
      }

      public async Task<List<User>> GetUsersAsync(int? pageSize = 20, CancellationToken cancellation = default)
      {
         var loop = 0;

         throw new Exception("GetUsersAsync => Error");

         while (!cancellation.IsCancellationRequested || loop < 5)
         {
            await Task.Delay(1000, cancellation).ContinueWith((x) =>
            {
               return Task.CompletedTask;
            });

            loop++;
         }

         return GetUsers(pageSize);
      }

      public bool SaveUsers(User user)
      {
         _logger.LogMessage("Ho scritto il log");

         return false;
      }
   }
}
