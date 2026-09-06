using FluentValidation;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Card.Models.RequestModel;

public class AssignCardRequestValidator : AbstractValidator<AssignCardRequestModel>
{
    public AssignCardRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithErrorCode(ErrorCodes.InvalidUsernameFormat);
    }
}