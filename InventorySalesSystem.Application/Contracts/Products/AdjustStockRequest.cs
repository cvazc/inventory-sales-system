namespace InventorySalesSystem.Application.Contracts.Products;

public class AdjustStockRequest
{
    public int Delta { get; set; }
    public string? Reason { get; set; }
}