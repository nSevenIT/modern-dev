using DataContext;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Extensions
{
   internal static class IServiceCollectionExtensions
   {
      /// <summary>
      /// Adds BpMes Database Context
      /// </summary>
      /// <param name="services">Service Collection</param>
      /// <param name="configuration">IConfiguration</param>
      internal static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
      {
         var connectionString = configuration.GetConnectionString("DefaultConnection");

         if (string.IsNullOrEmpty(connectionString)) throw new NullReferenceException(nameof(connectionString));

         services.AddDbContextPool<ApplicationDataContext>(options =>
         {
            options.UseSqlServer(connectionString, options =>
            {
               options.CommandTimeout(45);
               options.EnableRetryOnFailure(3, TimeSpan.FromMilliseconds(500), null);
            });
         });

         return services;
      }
   }
}
