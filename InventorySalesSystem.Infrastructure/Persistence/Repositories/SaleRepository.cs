using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Infrastructure.Persistence.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly InventoryDbContext _dbContext;

    public SaleRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> CountAsync(CancellationToken ct = default)
        => _dbContext.Sales.CountAsync(ct);

    public Task<List<Sale>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public Task<Sale?> GetByIdWithItemsAsync(int id, CancellationToken ct = default)
        => _dbContext.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddAsync(Sale sale, CancellationToken ct = default)
        => await _dbContext.Sales.AddAsync(sale, ct);

    public async Task LoadSaleItemsWithProductAsync(Sale sale, CancellationToken ct = default)
    {
        await _dbContext.Entry(sale)
            .Collection(s => s.Items)
            .Query()
            .Include(i => i.Product)
            .LoadAsync(ct);
    }

    public async Task<IDisposable> BeginTransactionAsync(CancellationToken ct = default)
        => await _dbContext.Database.BeginTransactionAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);

    public async Task CommitTransactionAsync(IDisposable transaction, CancellationToken ct = default)
    {
        if (transaction is Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTx)
            await dbTx.CommitAsync(ct);
        else
            transaction.Dispose();
    }
}
