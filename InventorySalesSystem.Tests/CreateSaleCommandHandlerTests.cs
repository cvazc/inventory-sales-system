using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Application.Features.Sales.Create;
using InventorySalesSystem.Domain.Entities;
using InventorySalesSystem.Tests.Fakes;
using Xunit;

public class CreateSaleCommandHandlerTests
{
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Stock_Is_Insufficient()
    {
        var productsRepo = new FakeProductRepository();
        productsRepo.Seed(new Product
        {
            Id = 1,
            Price = 100,
            StockQuantity = 0,
            IsActive = true
        });

        var salesRepo = new FakeSaleRepository();
        var publisher = new SaleEventPublisher();

        var handler = new CreateSaleCommandHandler(salesRepo, productsRepo, publisher);

        var request = new CreateSaleRequest
        {
            CustomerName = "Test",
            Items =
            {
                new CreateSaleItemRequest
                {
                    ProductId = 1,
                    Quantity = 1
                }
            }
        };

        await Assert.ThrowsAsync<BadRequestException>(
            () => handler.HandleAsync(new CreateSaleCommand(request))
        );
    }
}
