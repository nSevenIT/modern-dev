using Domain;

namespace ApplicationServices
{
   public interface IUsersService
   {
      Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
      Task<List<User>> GetUsersAsync(int pageNumber, int? pageSize = 20, CancellationToken cancellation = default);
      Task<Guid> CreateUsersAsync(User user, CancellationToken cancellation = default);

      Task<Guid> UpdateUsersAsync(User user, CancellationToken cancellation = default);

      Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
   }
}
