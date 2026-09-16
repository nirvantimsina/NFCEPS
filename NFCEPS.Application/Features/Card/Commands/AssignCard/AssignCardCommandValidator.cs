using FluentValidation;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardCommandValidator : AbstractValidator<AssignCardCommand>
    {
        public AssignCardCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("User ID must be a valid number");
        }
    }
}
