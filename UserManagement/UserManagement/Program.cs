using UserManagement.Services;

namespace UserManagement
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);

         //// Add services to the container.

         //builder.Services.AddAuthorization();

         builder.Services.AddScoped<IUsersService, UsersService>();
         builder.Services.AddSingleton<ILoggerService, LoggerService>();

         var app = builder.Build();

         // Configure the HTTP request pipeline.

         //app.UseHttpsRedirection();

         //app.UseAuthorization();

         app.MapGet("/api/users", GetUsers);  // Get list
         app.MapGet("/api/users/{id:int}", GetUser);  // Get specific
         app.MapPost("/api/users", SaveUser); // Create
         app.MapPut("/api/users", UpdateUser); // Update
         app.MapDelete("/api/users", DeleteUser); // Delete

         app.Run();
      }

      public static IResult GetUsers(IUsersService usersService, HttpContext httpContext)
      {
         var result = new List<User>();

         return Results.Ok(result);
      }

      public static IResult GetUser(int id, IUsersService usersService, HttpContext httpContext)
      {
         //var result = usersService.SaveUsers(new User());

         return Results.Ok(new User());
      }

      public static IResult SaveUser(User user, IUsersService usersService, HttpContext httpContext)
      {
         var result = usersService.SaveUsers(new User());

         return Results.Ok(result);
      }

      public static IResult UpdateUser(IUsersService usersService, HttpContext httpContext)
      {
         //var result = usersService.SaveUsers(new User());

         return Results.Ok(new User());
      }

      public static IResult DeleteUser(IUsersService usersService, HttpContext httpContext)
      {
         //var result = usersService.SaveUsers(new User());

         return Results.Ok(new User());
      }
   }
}
