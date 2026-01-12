namespace InventorySalesSystem.Api.Contracts.Sales;

public class CreateSaleRequest
{
    public string? CustomerName { get; set; }
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}