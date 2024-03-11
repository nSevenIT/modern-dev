using System.Diagnostics;

namespace UserManagement.Middlewares
{
   internal class ResponseTimeFilter : IEndpointFilter
   {
      private readonly IHostEnvironment _hostEnvironment;
      private readonly ILogger<ResponseTimeFilter> _logger;

      public ResponseTimeFilter(IHostEnvironment hostEnvironment, ILogger<ResponseTimeFilter> logger)
      {
         _hostEnvironment = hostEnvironment;
         _logger = logger;
      }

      public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
      {
         if (string.Equals(_hostEnvironment?.EnvironmentName, Environments.Production, StringComparison.OrdinalIgnoreCase))
         {
            return await next(context);
         }

         var stopwatch = Stopwatch.StartNew();

         var result = await next(context);

         stopwatch.Stop();

         _logger.LogInformation("{stopwatch} milliseconds", stopwatch.ElapsedMilliseconds);

         context.HttpContext.Response.Headers.Append("X-Response-Time", $"{stopwatch.ElapsedMilliseconds} milliseconds");

         return result;
      }
   }
}
