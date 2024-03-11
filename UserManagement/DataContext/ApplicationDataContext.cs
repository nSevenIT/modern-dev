using DataContext.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
   public sealed class ApplicationDataContext : DbContext
   {
      public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
      {
      }

      /// <summary>
      /// Users
      /// </summary>
      public DbSet<Domain.User> Users { get; set; }

      /// <summary>
      /// OnModelCreating
      /// </summary>
      /// <param name="modelBuilder">Model Builder</param>
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         if (modelBuilder is null) return;

         base.OnModelCreating(modelBuilder);

         modelBuilder.HasDefaultSchema("dbo");

         modelBuilder.ApplyConfiguration(new UsersConfiguration());
      }
   }
}
