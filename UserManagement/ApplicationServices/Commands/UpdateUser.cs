namespace ApplicationServices.Commands
{
   /// <summary>
   /// Updates a single users
   /// </summary>
   public sealed class UpdateUser
   {
      /// <summary>
      /// User id
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// User Name
      /// </summary>
      /// <example></example>
      public string Name { get; set; } = string.Empty;

      /// <summary>
      /// Email Address
      /// </summary>
      public string EmailAddress { get; set; } = string.Empty;
   }
}
