using FluentAssertions;
using FluentValidation;
using LoanAPI.Data;

namespace Finall_Project.Validators
{
        public class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
            RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First Name is required")
                    .MaximumLength(50).WithMessage("First Name is too long, enter less than 50 letters");
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Last Name is required")
                    .MaximumLength(50).WithMessage("Last Name is too long, enter less than 50 letters");
            RuleFor(x => x.Age)
                    .NotEmpty().WithMessage("Age is required")
                    .GreaterThanOrEqualTo(18).WithMessage("Age is not correct");
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("‘Email’ is not a valid email address")
                    .WithMessage("Age is not correct");
            RuleFor(x => x.Salary)
                    .InclusiveBetween(0, 10000)
                    .WithMessage("Incorrect salary amount, enter between 0-10.000");
            RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required")
                    .MaximumLength(50).WithMessage("Username is too long, enter less than 50 letters");
            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required")
                    .MinimumLength(8).WithMessage("Password should be more than 8 symbols");
            RuleFor(x => x.Role)
                    .NotEmpty().WithMessage("Role is required")
                    .Must(r => r == "User" || r == "Admin").WithMessage("Role must be User or Admin");
            RuleFor(x => x.Isblocked)
                    .NotEmpty().WithMessage("Status is required");
                    //.Must(r => r == "False" || r == "True").WithMessage("Isblock Status must be False or True");
        }       
        }
    
}
