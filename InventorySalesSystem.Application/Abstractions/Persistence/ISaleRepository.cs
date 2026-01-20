using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Abstractions.Persistence;

public interface ISaleRepository
{
    Task<int> CountAsync(CancellationToken ct = default);
    Task<List<Sale>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Sale?> GetByIdWithItemsAsync(int id, CancellationToken ct = default);

    Task AddAsync(Sale sale, CancellationToken ct = default);
    Task LoadSaleItemsWithProductAsync(Sale sale, CancellationToken ct = default);

    Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
