using FluentValidation;

namespace ApplicationServices.Commands
{
   public class UpdateUserValidator : FluentValidation.AbstractValidator<UpdateUser>
   {
      private readonly IUsersService _usersService;

      public UpdateUserValidator(IUsersService usersService)
      {
         _usersService = usersService;

         // ClassLevelCascadeMode = CascadeMode.Stop;

         RuleFor(f => f.Id)
            .NotEmpty()
                 .WithSeverity(Severity.Error)
                 .WithMessage("Id: Mandatory field.")
            .MustAsync(async (context, id, cancellationToken) =>
            {
               return await _usersService.UserExistsAsync(id, cancellationToken);
            })
            .WithSeverity(Severity.Error)
            .WithErrorCode("404")           // Not Found
            .WithMessage("User not found.");

         RuleFor(f => f.Name)
             .NotEmpty()
                 .WithSeverity(Severity.Error)
                 .WithMessage("Name: Mandatory field.")
             .Length(1, 20)
                 .WithSeverity(Severity.Error)
                 .WithMessage("Name: Max length 20.");

         RuleFor(f => f.EmailAddress)
             .NotEmpty()
                 .WithSeverity(Severity.Error)
                 .WithMessage("Name: Mandatory field.")
             .Length(1, 60)
                 .WithSeverity(Severity.Error)
                 .WithMessage("Name: Max length 60.");
      }
   }
}
