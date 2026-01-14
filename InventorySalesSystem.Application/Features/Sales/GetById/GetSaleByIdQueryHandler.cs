using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Features.Sales.GetById;

public sealed class GetSaleByIdQueryHandler
{
    private readonly ISaleRepository _sales;

    public GetSaleByIdQueryHandler(ISaleRepository sales)
    {
        _sales = sales;
    }

    public async Task<SaleResponse> HandleAsync(GetSaleByIdQuery query, CancellationToken ct = default)
    {
        var sale = await _sales.GetByIdWithItemsAsync(query.Id, ct);

        if (sale is null)
            throw new NotFoundException($"Sale with id {query.Id} was not found.");

        return SaleMapper.ToResponse(sale);
    }
}
