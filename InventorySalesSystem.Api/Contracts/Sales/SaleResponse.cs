namespace InventorySalesSystem.Api.Contracts.Sales;

public class SaleResponse
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemResponse> Items { get; set; } = new();
}