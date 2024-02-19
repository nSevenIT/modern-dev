namespace UserManagement.Options
{
   public sealed class ApplicationOptions
   {
      public const string SECTION_NAME = "Application";

      public string Name { get; set; } = "Test App";

      public Uri? Url { get; set; }

      public int DefaultPageSize { get; set; } = 100;
   }
}
