namespace InventorySalesSystem.Api.Events;

public record SaleCreatedEvent(int SaleId, decimal TotalAmount, DateTime CreatedAt);