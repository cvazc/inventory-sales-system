using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Contracts.Common;
using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Features.Sales.GetPaged;

public sealed class GetSalesPagedQueryHandler
{
    private readonly ISaleRepository _sales;

    public GetSalesPagedQueryHandler(ISaleRepository sales)
    {
        _sales = sales;
    }

    public async Task<PagedResult<SaleResponse>> HandleAsync(GetSalesPagedQuery query, CancellationToken ct = default)
    {
        var totalItems = await _sales.CountAsync(ct);
        var sales = await _sales.GetPagedAsync(query.Page, query.PageSize, ct);

        return new PagedResult<SaleResponse>
        {
            Items = sales.Select(ToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
        };
    }

    private static SaleResponse ToResponse(Sale sale)
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
