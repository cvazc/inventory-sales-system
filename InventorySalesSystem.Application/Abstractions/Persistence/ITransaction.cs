namespace InventorySalesSystem.Application.Abstractions.Persistence;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct = default);
}
