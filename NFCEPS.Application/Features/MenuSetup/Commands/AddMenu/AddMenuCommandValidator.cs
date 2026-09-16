using FluentValidation;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Commands.AddMenu;

public class AddMenuCommandValidator : AbstractValidator<AddMenuCommand>
{
    public AddMenuCommandValidator()
    {
        RuleFor(x => x.MenuName)
            .NotEmpty().WithErrorCode(ErrorCodes.MissingRequiredField)
            .Matches("^[a-zA-Z]+$").WithErrorCode(ErrorCodes.InvalidFormat);

        RuleFor(x => x.ParentId)
            .NotEmpty().WithErrorCode(ErrorCodes.MissingRequiredField)
            .GreaterThan(0).WithErrorCode(ErrorCodes.InvalidFormat);

        RuleFor(x => x.Icon)
            .Matches(@"^[a-zA-Z0-9/._\s-]+$").WithErrorCode(ErrorCodes.InvalidFormat);
        
        RuleFor(x => x.Path)
            .Matches(@"^[a-zA-Z0-9/.?=%-]+$").WithErrorCode(ErrorCodes.InvalidUriFormat);

        RuleFor(x => x.MenuOrder)
            .GreaterThan(0).WithErrorCode(ErrorCodes.InvalidFormat);
    }
}
