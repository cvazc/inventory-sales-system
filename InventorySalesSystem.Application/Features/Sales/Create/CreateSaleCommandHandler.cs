using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Features.Sales.Create;

public sealed class CreateSaleCommandHandler
{
    private readonly ISaleRepository _sales;
    private readonly IProductRepository _products;
    private readonly SaleEventPublisher _publisher;

    public CreateSaleCommandHandler(ISaleRepository sales, IProductRepository products, SaleEventPublisher publisher)
    {
        _sales = sales;
        _products = products;
        _publisher = publisher;
    }

    public async Task<SaleResponse> HandleAsync(CreateSaleCommand command, CancellationToken ct = default)
    {
        var request = command.Request;

        CreateSaleRequestNormalizer.Normalize(request);

        using var tx = await _sales.BeginTransactionAsync(ct);

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _products.GetByIdsAsync(productIds, ct);

        if (products.Count != productIds.Count)
            throw new BadRequestException("One or more products do not exist.");

        var sale = new Sale
        {
            CustomerName = request.CustomerName,
            CreatedAt = DateTime.UtcNow
        };

        decimal total = 0m;

        foreach (var reqItem in request.Items)
        {
            var product = products.Single(p => p.Id == reqItem.ProductId);

            if (!product.IsActive)
                throw new BadRequestException($"Product {product.Id} is not active.");

            if (product.StockQuantity < reqItem.Quantity)
                throw new BadRequestException($"Not enough stock for product {product.Id}. Current stock: {product.StockQuantity}.");

            product.StockQuantity -= reqItem.Quantity;

            var unitPrice = product.Price;
            var lineTotal = unitPrice * reqItem.Quantity;

            sale.Items.Add(new SaleItem
            {
                ProductId = product.Id,
                Quantity = reqItem.Quantity,
                UnitPrice = unitPrice,
                LineTotal = lineTotal
            });

            total += lineTotal;
        }

        sale.TotalAmount = total;

        await _sales.AddAsync(sale, ct);
        await _sales.SaveChangesAsync(ct);

        await _sales.CommitTransactionAsync(tx, ct);

        _publisher.PublishSaleCreated(new SaleCreatedEvent(sale.Id, sale.TotalAmount, sale.CreatedAt));

        await _sales.LoadSaleItemsWithProductAsync(sale, ct);

        return SaleMapper.ToResponse(sale);
    }
}
