using ApplicationServices;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Serilog;
using Serilog.Extensions.Hosting;
using UserManagement.Extensions;
using UserManagement.Features.Users;
using UserManagement.Middlewares;

namespace UserManagement
{
   public class Program
   {
      /// <summary>
      /// Create a Serilog logger
      /// </summary>
      /// <returns>Reloadable Logger</returns>
      private static ReloadableLogger CreateLogger() => new LoggerConfiguration()
              .CreateBootstrapLogger();

      public static void Main(string[] args)
      {
         Log.Logger = CreateLogger();

         var builder = WebApplication.CreateBuilder(args);

         //// Add services to the container.

         builder.Host.UseSerilog((context, configuration) => configuration
                .WriteTo.Console()
                .ReadFrom.Configuration(context.Configuration));

         builder.Services.AddProblemDetails();
         builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();

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

         builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                          .AddMicrosoftIdentityWebApi(options =>
                          {
                             builder.Configuration.Bind("AzureAd", options);
                             options.TokenValidationParameters.NameClaimType = "name";
                          }, options =>
                          {
                             builder.Configuration.Bind("AzureAd", options);
                          });

         builder.Services.AddAuthorizationBuilder()
             .AddPolicy("MyAuthorizationPolicy", policyBuilder =>
                policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement()
                {
                   RequiredScopesConfigurationKey = $"AzureAd:Scopes"
                }));

         // builder.Services.AddHttpContextAccessor();

         builder.Services.AddScoped<IUsersService, UsersService>();

         // Open Api Documentation
         if (builder.Environment.IsDevelopment())
         {
            builder.Services
                   .AddEndpointsApiExplorer()
                   .AddSwaggerGen(options =>
                   {
                      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ApplicationServices.xml"));
                      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Domain.xml"));
                   });
         }

         // Fluent Validation
         builder.Services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServices.Commands.CreateUser));

         var app = builder.Build();

         // Configure the HTTP request pipeline.

         // Middleware order
         // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0

         app.UseStatusCodePages();
         app.UseExceptionHandler();

#if DEBUG
         // Output all trafic in console
         app.UseSerilogRequestLogging();
#endif

         if (app.Environment.IsDevelopment())
         {
            app.UseSwagger()
               .UseSwaggerUI();
         }

         app.UseHttpsRedirection();

         // // app.UseCors();
         app.UseAuthorization();

         app.MapGroup("/api/users").MapUsersEndpoints()
             .WithTags("Users")
             .WithOpenApi()
             .WithMetadata()
             .AddEndpointFilter<ResponseTimeFilter>();

         // Swagger / OpenApi only in test
         if (app.Environment.IsDevelopment())
         {
            _ = app.MapGet("", context =>
            {
               context.Response.Redirect("./swagger/index.html", permanent: false);

               return Task.CompletedTask;
            });
         }

         app.Run();
      }
   }
}
