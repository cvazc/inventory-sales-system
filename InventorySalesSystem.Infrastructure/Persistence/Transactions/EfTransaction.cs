using InventorySalesSystem.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace InventorySalesSystem.Infrastructure.Persistence.Transactions;

public sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _tx;

    public EfTransaction(IDbContextTransaction tx)
    {
        _tx = tx;
    }

    public Task CommitAsync(CancellationToken ct = default)
        => _tx.CommitAsync(ct);

    public ValueTask DisposeAsync()
        => _tx.DisposeAsync();
}
