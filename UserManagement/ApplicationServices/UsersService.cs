using DataContext;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationServices
{
   public sealed class UsersService : IUsersService
   {
      private readonly ApplicationDataContext _databaseContext;
      private readonly ILogger<UsersService> _logger;

      public UsersService(ApplicationDataContext databaseContext, ILogger<UsersService> logger)
      {
         _databaseContext = databaseContext;
         _logger = logger;
      }

      public async Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
      {
         _logger.LogDebug("Lettura di utente con id {id}", id);

         var user = await _databaseContext
            .Users
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

         return user;
      }

      public Task<List<User>> GetUsersAsync(int pageNumber, int? pageSize = 20, CancellationToken cancellation = default)
      {
         _logger.LogDebug("Lettura di utenti pagina: {pageNumber} {pageSize}", pageNumber, pageSize);

         return _databaseContext.Users
               .TagWith(nameof(GetUsersAsync))
               .Skip(pageNumber)
               .Take(pageSize ?? 20)
               .OrderBy(x => x.Id)
               .ToListAsync(cancellation);
      }

      public async Task<Guid> CreateUsersAsync(User user, CancellationToken cancellation = default)
      {
         await _databaseContext.Users
            .AddAsync(user, cancellation);

         await _databaseContext
            .SaveChangesAsync(cancellation);

         return user.Id;
      }

      public async Task<Guid> UpdateUsersAsync(User user, CancellationToken cancellation = default)
      {
         var currentUser = await _databaseContext.Users.Where(x => x.Id == user.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellation);

         if (currentUser is not null)
         {

            user.CreatedBy = user.CreatedBy;
            user.CreatedAt = user.CreatedAt;

            _databaseContext.Users.Update(user);

            await _databaseContext.SaveChangesAsync(cancellation);

            return user.Id;
         }

         return Guid.Empty;
      }

      public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
      {
         _logger.LogDebug("Cancellazione di utente con id {id}", id);

         var user = await _databaseContext
            .Users
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

         if (user is not null)
         {
            user.Deleted = true;
            user.DeletedBy = "Admin";
            user.DeletedAt = DateTime.Now;

            _databaseContext.Users.Update(user);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
