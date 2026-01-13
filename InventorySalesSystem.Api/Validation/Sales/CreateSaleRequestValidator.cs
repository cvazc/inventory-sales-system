using FluentValidation;
using InventorySalesSystem.Application.Contracts.Sales;

namespace InventorySalesSystem.Api.Validation.Sales;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty()
            .WithMessage("A sale must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be a positive integer.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        });
    }
}
