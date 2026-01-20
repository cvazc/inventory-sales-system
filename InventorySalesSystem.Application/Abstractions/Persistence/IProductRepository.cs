using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Product>> GetByIdsAsync(List<int> ids, CancellationToken ct = default);

    Task AddAsync(Product product, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
