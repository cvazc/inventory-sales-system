namespace InventorySalesSystem.Application.Events;

public class SaleEventPublisher
{
    public event EventHandler<SaleCreatedEvent>? SaleCreated;
    
    public void PublishSaleCreated(SaleCreatedEvent payload)
    {
        SaleCreated?.Invoke(this, payload);
    }
}