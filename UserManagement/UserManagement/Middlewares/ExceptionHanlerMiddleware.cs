
using UserManagement.Services;

namespace UserManagement.Middlewares
{
   /// <summary>
   /// Factory-Activate Middleware
   /// </summary>
   public class ExceptionHanlerMiddleware : IMiddleware
   {
      private readonly ILoggerService _logger;

      public ExceptionHanlerMiddleware(ILoggerService logger)
      {
         _logger = logger;
      }

      public async Task InvokeAsync(HttpContext context, RequestDelegate next)
      {
         try
         {
            await next(context);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception);
         }
      }
   }
}
