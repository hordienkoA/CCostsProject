using System.Linq;
using CConstsProject.Models;
using FluentValidation;

namespace CCostsProject.Validation
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email).NotNull().WithMessage("Email is a required field")
                .Matches(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?").WithMessage("The email address not valid");
            RuleFor(user => user.UserName).NotNull().WithMessage("Username is a required field")
                .Matches("^[a-zA-Z0-9\\._-]*$").WithMessage("Username is not valid")
                .MinimumLength(3).WithMessage("Username must have 3 or more character")
                .MaximumLength(64).WithMessage("Username must have 64 or less characters");
            RuleFor(user => user.Password)
                .NotNull().WithMessage("Password is a required field")
                .Must(p => p.Any(char.IsDigit) && p.Any(char.IsUpper) && p.Any(char.IsLower))
                .WithMessage("Password in not valid");
            RuleFor(user => user.FirstName)
                
                .Matches("^[a-zA-Z\\- ]*$").When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("First name is not valid")
                .MinimumLength(1).When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("First name must have at least 1 character")
                .MaximumLength(255).When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("First name must have less then 255 characters");
            RuleFor(user=>user.SecondName)
                
                .Matches("^[a-zA-Z\\- ]*$").When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("Second name is not valid")
                .MinimumLength(1).When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("Second name must have at least 1 character")
                .MaximumLength(255).When(x=>!string.IsNullOrEmpty(x.FirstName)).WithMessage("Second name must have less then 255 characters");
        }
    }
}