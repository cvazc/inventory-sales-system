namespace InventorySalesSystem.Application.Features.Sales.GetPaged;

public sealed class GetSalesPagedQuery
{
    public int Page { get; }
    public int PageSize { get; }

    public GetSalesPagedQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
