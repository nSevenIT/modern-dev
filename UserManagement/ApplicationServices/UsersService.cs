using ApplicationServices.Commands;
using DataContext;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationServices
{
   /// <summary>
   /// 
   /// </summary>
   public sealed class UsersService : IUsersService
   {
      private readonly ApplicationDataContext _databaseContext;
      private readonly ILogger<UsersService> _logger;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="databaseContext"></param>
      /// <param name="logger"></param>
      public UsersService(ApplicationDataContext databaseContext, ILogger<UsersService> logger)
      {
         _databaseContext = databaseContext;
         _logger = logger;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="id"></param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      public async Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
      {
         _logger.LogDebug("{date}: Lettura di utente con id: {id}", DateTime.Now, id);

         var user = await _databaseContext
            .Users
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

         return user;
      }

      public async Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
      {
         return await _databaseContext
          .Users
          .Where(x => x.Id == id)
          .AnyAsync(cancellationToken);
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

      public async Task<Guid> CreateUsersAsync(CreateUser createUser, CancellationToken cancellation = default)
      {
         var user = new Domain.User
         {
            Name = createUser.Name,
            EmailAddress = createUser.EmailAddress,
            CreatedAt = DateTime.Now,
            CreatedBy = "Admin",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Admin"
         };

         await _databaseContext.Users
            .AddAsync(user, cancellation);

         await _databaseContext
            .SaveChangesAsync(cancellation);

         return user.Id;
      }

      public async Task<Guid> UpdateUsersAsync(UpdateUser user, CancellationToken cancellation = default)
      {
         var updateEachField = false;

         if (!updateEachField)
         {
            var currentUser = await _databaseContext.Users.Where(x => x.Id == user.Id)
                     .FirstOrDefaultAsync(cancellation);

            if (currentUser is not null)
            {
               currentUser.Name = user.Name;
               currentUser.EmailAddress = user.EmailAddress;
               currentUser.UpdatedBy = "Adin";
               currentUser.UpdatedAt = DateTime.Now;

               _databaseContext.Users.Update(currentUser);

               await _databaseContext.SaveChangesAsync(cancellation);
            }
         }
         else
         {
            var updatedUser = new Domain.User
            {
               Id = user.Id,
               Name = user.Name,
               EmailAddress = string.Empty,
               UpdatedAt = DateTime.Now
            };

            _databaseContext.Attach(updatedUser);

            _databaseContext.Entry(updatedUser).Property(x => x.Name).IsModified = true;
            _databaseContext.Entry(updatedUser).Property(x => x.UpdatedAt).IsModified = true;

            await _databaseContext.SaveChangesAsync(cancellation);
         }

         return user.Id;
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
