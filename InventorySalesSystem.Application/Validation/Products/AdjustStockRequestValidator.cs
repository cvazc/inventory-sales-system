using FluentValidation;
using InventorySalesSystem.Application.Contracts.Products;

namespace InventorySalesSystem.Application.Validation.Products;

public class AdjustStockRequestValidator : AbstractValidator<AdjustStockRequest>
{
    public AdjustStockRequestValidator()
    {
        RuleFor(x => x.Delta)
            .NotEqual(0)
            .WithMessage("Delta must not be zero.");
    }
}
