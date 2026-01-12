using FluentValidation;
using InventorySalesSystem.Api.Contracts.Products;

namespace InventorySalesSystem.Api.Validation.Products;

public class AdjustStockRequestValidator : AbstractValidator<AdjustStockRequest>
{
    public AdjustStockRequestValidator()
    {
        RuleFor(x => x.Delta)
            .NotEqual(0)
            .WithMessage("Delta must not be zero.");
    }
}
