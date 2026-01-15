using InventorySalesSystem.Application.Contracts.Sales;

namespace InventorySalesSystem.Application.Features.Sales.Create;

internal static class CreateSaleRequestNormalizer
{
    public static void Normalize(CreateSaleRequest request)
    {
        request.Items = request.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new CreateSaleItemRequest
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();
    }
}
