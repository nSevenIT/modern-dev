using ApplicationServices;
using ApplicationServices.Commands;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Middlewares;

namespace UserManagement.Features.Users
{
   internal static class UsersEndpoints
   {
      internal static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder groups)
      {
         groups.MapGet("", GetUsersAsync)
               .Produces(StatusCodes.Status200OK, typeof(List<Domain.User>))
               .ProducesValidationProblem(StatusCodes.Status400BadRequest)
               .ProducesProblem(StatusCodes.Status401Unauthorized)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
               .ProducesProblem(StatusCodes.Status500InternalServerError)
               .WithName("Get Users")
               .WithDescription("Get a list of users");
         //.RequireAuthorization("MyAuthorizationPolicy");

         groups.MapGet("{id:guid}", GetUserAsync);  // Get specific

         groups.MapPost("", CreateUserAync)
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Create Users")
            .WithDescription("Create a new user")
            .AddEndpointFilter<ValidationFilter<CreateUser>>();

         groups.MapPut("", UpdateUserAsync)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<UpdateUser>>();

         groups.MapDelete("{id:guid}", DeleteUserAsync);

         return groups;
      }

      internal static async Task<IResult> GetUsersAsync(
         [FromQuery] int pageNumber,
         IUsersService usersService,
         HttpContext httpContext,
         CancellationToken cancellationToken)
      {
         var users = await usersService.GetUsersAsync(pageNumber, 20, cancellationToken);

         return Results.Ok(users);
      }

      internal static async Task<IResult> GetUserAsync([FromRoute] Guid id, IUsersService usersService, CancellationToken cancellationToken)
      {
         var users = await usersService.GetUserAsync(id, cancellationToken);

         return Results.Ok(users);
      }

      internal static async Task<IResult> CreateUserAync([FromBody] CreateUser user,
         IUsersService usersService,
         CancellationToken cancellationToken)
      {

         //var newUser = new Domain.User(user.Name, user.EmailAddress)
         //{
         //   CreatedAt = DateTime.Now,
         //   CreatedBy = "Admin",
         //   UpdatedAt = DateTime.Now,
         //   UpdatedBy = "Admin",
         //};

         var result = await usersService.CreateUsersAsync(user, cancellationToken);

         return Results.Created($"/api/users/{result}", null);
      }

      internal static async Task<IResult> UpdateUserAsync([FromBody] UpdateUser user, IUsersService usersService, CancellationToken cancellationToken)
      {
         var result = await usersService.UpdateUsersAsync(user, cancellationToken);

         return Results.Ok(result);
      }

      internal static async Task<IResult> DeleteUserAsync([FromRoute] Guid id, IUsersService usersService, CancellationToken cancellation)
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
}
