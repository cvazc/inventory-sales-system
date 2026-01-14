using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Tests.Fakes;

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(_products.ToList());

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<List<Product>> GetByIdsAsync(List<int> ids, CancellationToken ct = default)
        => Task.FromResult(_products.Where(p => ids.Contains(p.Id)).ToList());

    public Task AddAsync(Product product, CancellationToken ct = default)
    {
        if (product.Id == 0)
            product.Id = _nextId++;

        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => Task.CompletedTask;

    public void Seed(params Product[] products)
    {
        foreach (var p in products)
        {
            if (p.Id == 0) p.Id = _nextId++;
            _products.Add(p);
        }
    }
}
