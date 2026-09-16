using FluentValidation;

namespace NFCEPS.Application.Features.Hardware.DeductBalance.Commands;

public class DeductBalanceCommandValidator : AbstractValidator<DeductBalanceCommand>
{
    public DeductBalanceCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0).WithMessage("Card ID cannot be a negative integer!");

        RuleFor(x => x.Punch)
            .GreaterThan(0).WithMessage("Punch value cannot be a negative integer!");

        RuleFor(x => x.From)
            .GreaterThan(0).WithMessage("From location cannot be a negative integer!");

        RuleFor(x => x.To)
            .GreaterThan(0).WithMessage("To location cannot be a negative integer!");
        
        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("Entity ID cannot be a negative integer!");
    }
}
