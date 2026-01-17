using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Tests.Fakes;

public class FakeSaleRepository : ISaleRepository
{
    private readonly List<Sale> _sales = new();
    private int _nextId = 1;

    public Task<int> CountAsync(CancellationToken ct = default)
        => Task.FromResult(_sales.Count);

    public Task<List<Sale>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => Task.FromResult(_sales.ToList());

    public Task<Sale?> GetByIdWithItemsAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_sales.FirstOrDefault(s => s.Id == id));

    public Task AddAsync(Sale sale, CancellationToken ct = default)
    {
        if (sale.Id == 0)
            sale.Id = _nextId++;

        _sales.Add(sale);
        return Task.CompletedTask;
    }

    public Task LoadSaleItemsWithProductAsync(Sale sale, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
    => Task.FromResult<ITransaction>(new FakeTransaction());

    public Task SaveChangesAsync(CancellationToken ct = default)
        => Task.CompletedTask;

    private sealed class FakeTransaction : ITransaction
    {
        public Task CommitAsync(CancellationToken ct = default) => Task.CompletedTask;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
