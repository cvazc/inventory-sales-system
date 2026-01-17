using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InventoryDbContext _dbContext;

    public UserRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task AddAsync(AppUser user, CancellationToken ct = default)
        => await _dbContext.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);
}
