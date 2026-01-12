using InventorySalesSystem.Api.Models;

namespace InventorySalesSystem.Api.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> AdjustStockAsync(int productId, int delta);
}