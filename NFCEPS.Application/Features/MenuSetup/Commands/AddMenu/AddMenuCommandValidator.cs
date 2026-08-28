using FluentValidation;
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.MenuSetup.Commands.AddMenu;

public class AddMenuCommandValidator : AbstractValidator<AddMenuCommand>
{
    public AddMenuCommandValidator()
    {
        RuleFor(x => x.MenuName)
            .NotEmpty().WithErrorCode(ErrorCodes.MissingRequiredField)
            .Matches("^[a-zA-Z]+$").WithErrorCode(ErrorCodes.InvalidFormat);
    }
}
