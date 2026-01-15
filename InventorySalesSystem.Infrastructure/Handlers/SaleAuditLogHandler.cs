using InventorySalesSystem.Application.Events;
using Microsoft.Extensions.Logging;

namespace InventorySalesSystem.Infrastructure.Handlers;

public class SaleAuditLogHandler
{
    private readonly ILogger<SaleAuditLogHandler> _logger;

    public SaleAuditLogHandler(ILogger<SaleAuditLogHandler> logger)
    {
        _logger = logger;
    }

    public void OnSaleCreated(object? sender, SaleCreatedEvent e)
    {
        _logger.LogInformation("AUDIT - SaleCreated: SaleId={SaleId}, Total={TotalAmount}, CreatedAt={CreatedAt}",
            e.SaleId, e.TotalAmount, e.CreatedAt);
    }
}