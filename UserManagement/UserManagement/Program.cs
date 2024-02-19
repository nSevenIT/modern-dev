using Microsoft.AspNetCore.Mvc;
using UserManagement.Middlewares;
using UserManagement.Services;

namespace UserManagement
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);

         //// Add services to the container.

         builder.Services.AddProblemDetails();
         builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
         // builder.Services.AddTransient<ExceptionHanlerMiddleware>();

         // Opzioni definite in appSettings.<env>.json
         builder.Services
            .AddOptions<Options.ApplicationOptions>()
            .BindConfiguration(Options.ApplicationOptions.SECTION_NAME)
            .Validate(option =>
            {
               return !string.IsNullOrWhiteSpace(option.Name);
            });

         builder.Services.AddScoped<IUsersService, UsersService>();
         builder.Services.AddSingleton<ILoggerService, LoggerService>();

         var app = builder.Build();

         // Configure the HTTP request pipeline.

         // Middleware order
         // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0

         app.UseStatusCodePages();
         app.UseExceptionHandler();

         // app.UseMiddleware<ExceptionHanlerMiddleware>();
         // app.UseMiddleware<ResponseTimeMiddleware>();

         //// 2. Middleware
         //app.Use(async (context, next) =>
         //{
         //   await next();
         //   await context.Response.WriteAsync("Map Test 1");
         //});

         app.MapGet("/api/users", GetUsers);  // Get list
         app.MapGet("/api/users/{id:int}", GetUser);  // Get specific
         app.MapPost("/api/users", SaveUser); // Create
         app.MapPut("/api/users", UpdateUser); // Update
         app.MapDelete("/api/users", DeleteUser); // Delete

         app.Run();
      }

      public static async Task<IResult> GetUsers([FromQuery] int pageNumber,
         IUsersService usersService,
         HttpContext httpContext,
         CancellationToken cancellationToken)
      {
         var users = await usersService.GetUsersAsync(pageNumber, cancellationToken);

         return Results.Ok(users);
      }

      public static IResult GetUser([FromRoute] int id, IUsersService usersService, HttpContext httpContext)
      {


         return Results.Ok(new User(2));
      }

      public static IResult SaveUser(User user, IUsersService usersService, HttpContext httpContext)
      {
         var result = usersService.SaveUsers(new User(3));

         return Results.Ok(result);
      }

      public static IResult UpdateUser(IUsersService usersService, HttpContext httpContext)
      {
         //var result = usersService.SaveUsers(new User());

         return Results.Ok(new User(4));
      }

      public static IResult DeleteUser(IUsersService usersService, HttpContext httpContext)
      {
         //var result = usersService.SaveUsers(new User());

         return Results.Ok(new User(5));
      }
   }
}
