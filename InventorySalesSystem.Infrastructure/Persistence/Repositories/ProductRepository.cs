using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _dbContext;

    public ProductRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => _dbContext.Products.ToListAsync(ct);

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Product>> GetByIdsAsync(List<int> ids, CancellationToken ct = default)
        => _dbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _dbContext.Products.AddAsync(product, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);
}
