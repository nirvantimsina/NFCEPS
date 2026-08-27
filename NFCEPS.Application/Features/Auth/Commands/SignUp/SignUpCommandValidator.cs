using FluentValidation;

namespace NFCEPS.Application.Features.Auth.Commands.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.UserName)
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Username cannot contain spaces or special characters!");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be of minimum 8 characters!");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty!");

            RuleFor(x => x.Phone)
                .Length(10).WithMessage("Phone number can only be of exactly 10 numbers!")
                .Matches("^[0-9]+$").WithMessage("Phone number can only contain numbers!");
        }
    }
}
