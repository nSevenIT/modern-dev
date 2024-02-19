namespace UserManagement.Services
{
   public interface IUsersService
   {
      bool SaveUsers(User user);
      List<User> GetUsers(int? pageSize = 0);

      Task<List<User>> GetUsersAsync(int? pageSize = 20, CancellationToken cancellation = default);
   }
}
