namespace UserManagement
{
   /// <summary>
   /// User Class
   /// </summary>
   public class User
   {
      public User(int id)
      {
         Id = id;
      }

      /// <summary>
      /// Unique Id of the user
      /// </summary>
      public int Id { get; init; }
   }
}
