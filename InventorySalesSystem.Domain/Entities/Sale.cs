namespace InventorySalesSystem.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItem> Items { get; set; } = new();
}