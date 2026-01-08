using InventorySalesSystem.Api.Contracts.Sales;
using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Api.Services;

public class SaleService
{
    private readonly InventoryDbContext _dbContext;

    public SaleService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SaleResponse>> GetAllAsync()
    {
        var sales = await _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.Id)
            .ToListAsync();

        return sales.Select(ToResponse).ToList();
    }

    public async Task<SaleResponse> CreateAsync(CreateSaleRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            throw new ArgumentException("A sale must contain at least one item.");
        }

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            throw new ArgumentException("One or more products to not exist.");
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
                throw new ArgumentException($"Product {product.Id} is not active.");
            }

            if (product.StockQuantity < reqItem.Quantity)
            {
                throw new ArgumentException($"Not enough stock for product {product.Id}. Current stock: {product.StockQuantity}.");
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

        return ToResponse(sale);
    }

    public async Task<SaleResponse> GetByIdAsync(int id)
    {
        var sale = await _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sale is null ? null : ToResponse(sale);
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