using InventorySalesSystem.Infrastructure.Persistence;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Application.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task AdjustStockAsync_ShouldThrow_WhenStockWouldGoBelowZero()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductServiceTests_AdjustStockBelowZero")
            .Options;

        using var dbContext = new InventoryDbContext(options);

        dbContext.Products.Add(new Product
        {
            Sku = "PROD-TEST",
            Name = "Test Product",
            StockQuantity = 2,
            Price = 10,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();

        var service = new ProductService(dbContext);

        await Assert.ThrowsAsync<BadRequestException>(() => service.AdjustStockAsync(productId: 1, delta: -999));
    }
}
