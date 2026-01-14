using InventorySalesSystem.Application.Services;
using InventorySalesSystem.Tests.Fakes;
using InventorySalesSystem.Domain.Entities;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task AdjustStockAsync_Should_Not_Allow_Below_Zero()
    {
        var repo = new FakeProductRepository();
        repo.Seed(new Product { Id = 1, StockQuantity = 0, IsActive = true });

        var service = new ProductService(repo);

        await Assert.ThrowsAsync<InventorySalesSystem.Application.Exceptions.BadRequestException>(
            () => service.AdjustStockAsync(1, -1)
        );
    }
}
