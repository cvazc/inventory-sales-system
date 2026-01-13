using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Contracts.Common;
using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Application.Services.Interfaces;

namespace InventorySalesSystem.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _sales;
    private readonly IProductRepository _products;
    private readonly SaleEventPublisher _saleEventPublisher;

    public SaleService(
        ISaleRepository sales,
        IProductRepository products,
        SaleEventPublisher saleEventPublisher)
    {
        _sales = sales;
        _products = products;
        _saleEventPublisher = saleEventPublisher;
    }

    public async Task<PagedResult<SaleResponse>> GetAllAsync(int page, int pageSize)
    {
        var totalItems = await _sales.CountAsync();
        var sales = await _sales.GetPagedAsync(page, pageSize);

        return new PagedResult<SaleResponse>
        {
            Items = sales.Select(ToResponse).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

    }

    public async Task<SaleResponse> CreateAsync(CreateSaleRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            throw new BadRequestException("A sale must contain at least one item.");
        }

        request.Items = request.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new CreateSaleItemRequest
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        foreach (var item in request.Items)
        {
            if (item.ProductId <= 0)
            {
                throw new BadRequestException("ProductId must be a positive integer.");
            }

            if (item.Quantity <= 0)
            {
                throw new BadRequestException("Quantity must be greater than zero.");
            }
        }

        using var transaction = await _sales.BeginTransactionAsync();

        var productIds = request.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();
        var products = await _products.GetByIdsAsync(productIds);

        if (products.Count != productIds.Count)
        {
            throw new BadRequestException("One or more products do not exist.");
        }

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
            {
                throw new BadRequestException($"Product {product.Id} is not active.");
            }

            if (product.StockQuantity < reqItem.Quantity)
            {
                throw new BadRequestException($"Not enough stock for product {product.Id}. Current stock: {product.StockQuantity}.");
            }

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

        await _sales.AddAsync(sale);
        await _sales.SaveChangesAsync();
        await _sales.CommitTransactionAsync(transaction);

        _saleEventPublisher.PublishSaleCreated(
            new SaleCreatedEvent(sale.Id, sale.TotalAmount, sale.CreatedAt)
        );

        await _sales.LoadSaleItemsWithProductAsync(sale);

        return ToResponse(sale);
    }

    public async Task<SaleResponse> GetByIdAsync(int id)
    {
        var sale = await _sales.GetByIdWithItemsAsync(id);

        if (sale is null)
        {
            throw new NotFoundException($"Sale with id {id} was not found.");
        }

        return ToResponse(sale);

    }

    private static SaleResponse ToResponse(Sale sale)
    {
        return new SaleResponse
        {
            Id = sale.Id,
            CustomerName = sale.CustomerName,
            CreatedAt = sale.CreatedAt,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.LineTotal
            }).ToList()
        };
    }
}