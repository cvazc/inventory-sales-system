namespace InventorySalesSystem.Application.Features.Sales.GetById;

public sealed class GetSaleByIdQuery
{
    public int Id { get; }

    public GetSaleByIdQuery(int id)
    {
        Id = id;
    }
}
