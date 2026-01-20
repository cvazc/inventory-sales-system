using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(AppUser user, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
