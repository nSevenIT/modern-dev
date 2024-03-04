using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Configuration
{
   /// <summary>
   /// Non obbligatoria.
   /// </summary>
   internal class UsersConfiguration : IEntityTypeConfiguration<User>
   {
      public void Configure(EntityTypeBuilder<User> builder)
      {
         builder.ToTable($"{nameof(User)}s");

         builder.HasKey(p => p.Id)
                .HasName("PK_Users");

         builder.HasQueryFilter(e => !e.Deleted);
      }
   }
}
