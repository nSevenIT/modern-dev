namespace ApplicationServices.Commands
{
   /// <summary>
   /// Payload to create a new user
   /// </summary>
   public class CreateUser
   {
      /// <summary>
      /// Username
      /// </summary>
      /// <example>test</example>
      public string Name { get; set; } = string.Empty;

      /// <summary>
      /// Email Address
      /// </summary>
      /// <example>test@test.com</example>
      public string EmailAddress { get; set; } = string.Empty;
   }
}
