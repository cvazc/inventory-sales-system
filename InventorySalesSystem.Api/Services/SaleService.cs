using InventorySalesSystem.Api.Contracts.Common;
using InventorySalesSystem.Api.Contracts.Sales;
using InventorySalesSystem.Infrastructure.Persistence;
using InventorySalesSystem.Api.Events;
using InventorySalesSystem.Api.Exceptions;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Api.Services;

public class SaleService : ISaleService
{
    private readonly InventoryDbContext _dbContext;
    private readonly SaleEventPublisher _saleEventPublisher;

    public SaleService(InventoryDbContext dbContext, SaleEventPublisher saleEventPublisher)
    {
        _dbContext = dbContext;
        _saleEventPublisher = saleEventPublisher;
    }

    public async Task<PagedResult<SaleResponse>> GetAllAsync(int page, int pageSize)
    {
        if (page <= 0)
        {
            throw new BadRequestException("Page must be greater than zero.");
        }

        if (pageSize <= 0 || pageSize > 100)
        {
            throw new BadRequestException("PageSize must be between 1 and 100.");
        }

        var query = _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.Id)
            .AsQueryable();

        var totalItems = await query.CountAsync();

        var sales = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<SaleResponse>
        {
            Items = sales.Select(ToResponse).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        return result;
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

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            throw new BadRequestException("One or more products to not exist.");
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

        _dbContext.Sales.Add(sale);
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        _saleEventPublisher.PublishSaleCreated(
            new SaleCreatedEvent(sale.Id, sale.TotalAmount, sale.CreatedAt)
        );

        await _dbContext.Entry(sale)
            .Collection(s => s.Items)
            .Query()
            .Include(i => i.Product)
            .LoadAsync();

        return ToResponse(sale);
    }

    public async Task<SaleResponse> GetByIdAsync(int id)
    {
        var sale = await _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

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