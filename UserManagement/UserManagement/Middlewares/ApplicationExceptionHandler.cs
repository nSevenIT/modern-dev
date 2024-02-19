using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services;

namespace UserManagement.Middlewares
{
   public class ApplicationExceptionHandler : IExceptionHandler
   {
      private readonly ILoggerService _logger;

      public ApplicationExceptionHandler(ILoggerService logger)
      {
         _logger = logger;
      }

      public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
      {
         _logger.LogError(exception);

         httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

         await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
         {
            Title = "An error occurred",
            Detail = exception.Message,
            Type = exception.GetType().Name,
            Status = (int)HttpStatusCode.InternalServerError
         }, cancellationToken: cancellationToken);

         return true;
      }
   }
}
