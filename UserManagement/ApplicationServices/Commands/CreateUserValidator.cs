using FluentValidation;

namespace ApplicationServices.Commands
{
   public sealed class CreateUserValidator : FluentValidation.AbstractValidator<CreateUser>
   {
      public CreateUserValidator()
      {
         // ClassLevelCascadeMode = CascadeMode.Stop;

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
