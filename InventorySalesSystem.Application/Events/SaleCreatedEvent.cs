namespace InventorySalesSystem.Application.Events;

public record SaleCreatedEvent(int SaleId, decimal TotalAmount, DateTime CreatedAt);