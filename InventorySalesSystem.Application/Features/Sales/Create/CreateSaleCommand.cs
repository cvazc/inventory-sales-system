using InventorySalesSystem.Application.Contracts.Sales;

namespace InventorySalesSystem.Application.Features.Sales.Create;

public sealed class CreateSaleCommand
{
    public CreateSaleRequest Request { get; }

    public CreateSaleCommand(CreateSaleRequest request)
    {
        Request = request;
    }
}