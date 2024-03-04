using System.Diagnostics.CodeAnalysis;

namespace Domain
{
   public class User
   {
      public User()
      {

      }

      [SetsRequiredMembers]
      public User(string name, string emailAddress)
      {
         Name = name;
         EmailAddress = emailAddress;
      }

      /// <summary>
      /// Primary Key
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Name of the user
      /// </summary>
      public required string Name { get; init; } = string.Empty;

      /// <summary>
      /// Email Address
      /// </summary>
      public required string EmailAddress { get; init; } = string.Empty;

      /// <summary>
      /// UTC Date of Creation
      /// </summary>
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      /// <summary>
      /// Created by user
      /// </summary>
      public string CreatedBy { get; set; } = string.Empty;

      /// <summary>
      /// UTC Date of Update
      /// </summary>
      public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

      /// <summary>
      /// Updated by user
      /// </summary>
      public string UpdatedBy { get; set; } = string.Empty;

      /// <summary>
      /// Deleted
      /// </summary>
      public bool Deleted { get; set; } = false;

      /// <summary>
      /// UTC Date of Delete
      /// </summary>
      public DateTime? DeletedAt { get; set; }

      /// <summary>
      /// Deleted by user 
      /// </summary>
      public string? DeletedBy { get; set; }
   }
}
