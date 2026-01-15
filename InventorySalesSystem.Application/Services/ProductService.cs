using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Application.Services.Interfaces;

namespace InventorySalesSystem.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _products.GetAllAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.IsActive = true;

        await _products.AddAsync(product);
        await _products.SaveChangesAsync();

        return product;
    }

    public async Task<Product> AdjustStockAsync(int productId, int delta)
    {
        var product = await _products.GetByIdAsync(productId);

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

        await _products.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _products.GetByIdAsync(id);
    }
}
