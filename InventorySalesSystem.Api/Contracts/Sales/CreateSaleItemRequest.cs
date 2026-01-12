namespace InventorySalesSystem.Api.Contracts.Sales;

public class CreateSaleItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
