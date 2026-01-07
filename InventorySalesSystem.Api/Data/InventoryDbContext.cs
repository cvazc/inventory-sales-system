using InventorySalesSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySalesSystem.Api.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
}