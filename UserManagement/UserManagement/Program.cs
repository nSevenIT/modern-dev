using ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Extensions;
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

         builder.Services
                .AddDatabaseContext(builder.Configuration)
                .AddDatabaseDeveloperPageExceptionFilter();

         builder.Services.AddScoped<IUsersService, UsersService>();
         builder.Services.AddSingleton<ILoggerService, LoggerService>();

         var app = builder.Build();

         // Configure the HTTP request pipeline.

         // Middleware order
         // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0

         app.UseStatusCodePages();
         app.UseExceptionHandler();

         app.MapGet("/api/users", GetUsersAsync);  // Get list
         app.MapGet("/api/users/{id:guid}", GetUserAsync);  // Get specific
         app.MapPost("/api/users", CreateUserAync); // Create
         app.MapPut("/api/users", UpdateUserAsync); // Update
         app.MapDelete("/api/users/{id:guid}", DeleteUserAsync); // Delete

         app.Run();
      }

      public static async Task<IResult> GetUsersAsync(
         [FromQuery] int pageNumber,
         IUsersService usersService,
         HttpContext httpContext,
         CancellationToken cancellationToken)
      {
         var users = await usersService.GetUsersAsync(pageNumber, 20, cancellationToken);

         return Results.Ok(users);
      }

      public static async Task<IResult> GetUserAsync([FromRoute] Guid id, IUsersService usersService, CancellationToken cancellationToken)
      {
         var users = await usersService.GetUserAsync(id, cancellationToken);

         return Results.Ok(users);
      }

      public static async Task<IResult> CreateUserAync([FromBody] CreateUser user,
         IUsersService usersService,
         CancellationToken cancellationToken)
      {

         var newUser = new Domain.User(user.Name, user.EmailAddress)
         {
            CreatedAt = DateTime.Now,
            CreatedBy = "Admin",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Admin",
         };

         var result = await usersService.CreateUsersAsync(newUser, cancellationToken);

         return Results.Created($"/api/users/{newUser.Id}", newUser);
      }

      public static async Task<IResult> UpdateUserAsync([FromBody] UpdateUser user, IUsersService usersService, CancellationToken cancellationToken)
      {
         var oldUser = new Domain.User(user.Name, user.EmailAddress)
         {
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Admin",
         };

         oldUser.Id = user.Id;

         var result = await usersService.UpdateUsersAsync(oldUser, cancellationToken);

         return Results.Ok();
      }

      public static async Task<IResult> DeleteUserAsync([FromRoute] Guid id, IUsersService usersService, CancellationToken cancellation)
      {
         var result = await usersService.DeleteUserAsync(id, cancellation);

         if (result)
         {
            return Results.Ok();
         }
         else
         {
            return Results.BadRequest();
         }
      }
   }

   public class CreateUser
   {
      public string Name { get; set; } = string.Empty;

      public string EmailAddress { get; set; } = string.Empty;

   }

   public class UpdateUser
   {
      public Guid Id { get; set; }

      public string Name { get; set; } = string.Empty;

      public string EmailAddress { get; set; } = string.Empty;

   }
}
