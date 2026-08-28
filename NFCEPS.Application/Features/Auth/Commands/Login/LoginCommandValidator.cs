using ErrorOr;
using FluentValidation;
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .Matches("^[a-zA-Z0-9]+$").WithMessage(ErrorCodes.InvalidFormat);
        
        RuleFor(x => x.Password)
            .MinimumLength(8).WithErrorCode(ErrorCodes.InvalidFormat);
    }
}
