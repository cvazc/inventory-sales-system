using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Api.Services;

public class ProductService
{
    private readonly InventoryDbContext _dbContext;

    public ProductService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }
}