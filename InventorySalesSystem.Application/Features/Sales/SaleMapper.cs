using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Features.Sales;

internal static class SaleMapper
{
    public static SaleResponse ToResponse(Sale sale)
    {
        return new SaleResponse
        {
            Id = sale.Id,
            CustomerName = sale.CustomerName,
            CreatedAt = sale.CreatedAt,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.LineTotal
            }).ToList()
        };
    }
}
