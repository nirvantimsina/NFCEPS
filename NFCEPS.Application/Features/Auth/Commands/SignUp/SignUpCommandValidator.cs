using FluentValidation;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Auth.Commands.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.UserName)
                .Matches("^[a-zA-Z0-9]+$").WithErrorCode(ErrorCodes.InvalidUsernameFormat);

            RuleFor(x => x.Password)
                .MinimumLength(8).WithErrorCode(ErrorCodes.InvalidPasswordFormat);
            
            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ErrorCodes.MissingRequiredField);

            RuleFor(x => x.Address)
                .NotEmpty().WithErrorCode(ErrorCodes.MissingRequiredField)
                .Matches("^[a-zA-Z0-9]+$").WithErrorCode(ErrorCodes.InvalidFormat);

            RuleFor(x => x.Phone)
                .Length(10).WithErrorCode(ErrorCodes.InvalidPhoneNoFormat)
                .Matches("^[0-9]+$").WithErrorCode(ErrorCodes.OnlyInteger);
        }
    }
}
