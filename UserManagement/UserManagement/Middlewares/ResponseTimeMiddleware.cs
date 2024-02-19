using System.Diagnostics;

namespace UserManagement.Middlewares
{
   /// <summary>
   /// Middleware activated by convention
   /// </summary>
   public class ResponseTimeMiddleware
   {
      private IHostEnvironment _env;
      private readonly RequestDelegate _next;

      public ResponseTimeMiddleware(IHostEnvironment env, RequestDelegate next)
      {
         _env = env;
         _next = next;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         if (_env.IsProduction())
         {
            await _next(context);

            return;
         }

         var stopwatch = new Stopwatch();
         stopwatch.Start();

         context.Response.OnStarting(() =>
         {
            context.Response.Headers["X-Response-Time"] = $"{stopwatch.ElapsedMilliseconds} ms";

            return Task.CompletedTask;
         });

         await _next(context);
      }
   }
}
