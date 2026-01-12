using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Exceptions;
using InventorySalesSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using InventorySalesSystem.Api.Services.Interfaces;

namespace InventorySalesSystem.Api.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _dbContext;

    public ProductService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.IsActive = true;

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product> AdjustStockAsync(int productId, int delta)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            throw new NotFoundException($"Product with id {productId} was not found.");
        }

        var newStock = product.StockQuantity + delta;

        if (newStock < 0)
        {
            throw new BadRequestException("Stock cannot go below zero.");
        }

        product.StockQuantity = newStock;
        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return product;
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}