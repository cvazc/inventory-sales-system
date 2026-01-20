using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventorySalesSystem.Application.Abstractions.Security;

namespace InventorySalesSystem.Infrastructure.Persistence.Seeding;

public sealed class DatabaseSeeder
{
    private readonly InventoryDbContext _db;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher _hasher;

    public DatabaseSeeder(InventoryDbContext db, IConfiguration config, IPasswordHasher hasher)
    {
        _db = db;
        _config = config;
        _hasher = hasher;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await SeedAdminAsync(ct);
        await SeedProductsAsync(ct);
    }

    private async Task SeedAdminAsync(CancellationToken ct)
    {
        var adminEmail = _config["Seed:Admin:Email"];
        if (string.IsNullOrWhiteSpace(adminEmail))
        {
            return;
        }

        var exists = await _db.Users.AnyAsync(u => u.Email == adminEmail, ct);
        if (exists) return;

        var adminPassword = _config["Seed:Admin:Password"];
        var adminPasswordHash = _config["Seed:Admin:PasswordHash"];

        string passwordHash;
        if (!string.IsNullOrWhiteSpace(adminPassword))
        {
            passwordHash = _hasher.Hash(adminPassword);
        }
        else if (!string.IsNullOrWhiteSpace(adminPasswordHash))
        {
            passwordHash = adminPasswordHash;
        }
        else
        {
            return;
        }

        var admin = new AppUser
        {
            Email = adminEmail,
            PasswordHash = passwordHash,
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(admin);
        await _db.SaveChangesAsync(ct);
    }

    private async Task SeedProductsAsync(CancellationToken ct)
    {
        var enabled = _config["Seed:Products:Enabled"];
        if (!string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var now = DateTime.UtcNow;

        var demoProducts = new[]
        {
        new Product
        {
            Sku = "DEMO-001",
            Name = "Demo Product A",
            Description = "Seeded demo product",
            StockQuantity = 100,
            Price = 49.99m,
            IsActive = true,
            CreatedAt = now
        },
        new Product
        {
            Sku = "DEMO-002",
            Name = "Demo Product B",
            Description = "Seeded demo product",
            StockQuantity = 50,
            Price = 79.99m,
            IsActive = true,
            CreatedAt = now
        },
        new Product
        {
            Sku = "DEMO-003",
            Name = "Demo Product C",
            Description = "Seeded demo product",
            StockQuantity = 25,
            Price = 99.99m,
            IsActive = true,
            CreatedAt = now
        }
    };

        foreach (var p in demoProducts)
        {
            var exists = await _db.Products.AnyAsync(x => x.Sku == p.Sku, ct);
            if (!exists)
            {
                _db.Products.Add(p);
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}
