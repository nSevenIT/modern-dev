using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Middlewares
{
   /// <summary>
   /// TODO Use ILogger
   /// </summary>
   public class ApplicationExceptionHandler : IExceptionHandler
   {
      private readonly ILogger<ApplicationExceptionHandler> _logger;

      public ApplicationExceptionHandler(ILogger<ApplicationExceptionHandler> logger)
      {
         _logger = logger;
      }

      public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
      {
         _logger.LogError(exception, "");

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
