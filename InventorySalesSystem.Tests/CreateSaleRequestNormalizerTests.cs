using InventorySalesSystem.Application.Contracts.Sales;
using InventorySalesSystem.Application.Features.Sales.Create;
using Xunit;

public class CreateSaleRequestNormalizerTests
{
    [Fact]
    public void Normalize_Should_Consolidate_Duplicate_ProductIds_By_Summing_Quantities()
    {
        // Arrange
        var request = new CreateSaleRequest
        {
            CustomerName = "Test",
            Items =
            {
                new CreateSaleItemRequest { ProductId = 1, Quantity = 2 },
                new CreateSaleItemRequest { ProductId = 1, Quantity = 3 },
                new CreateSaleItemRequest { ProductId = 2, Quantity = 1 }
            }
        };

        // Act
        CreateSaleRequestNormalizer.Normalize(request);

        // Assert
        Assert.Equal(2, request.Items.Count);

        var p1 = Assert.Single(request.Items, i => i.ProductId == 1);
        Assert.Equal(5, p1.Quantity);

        var p2 = Assert.Single(request.Items, i => i.ProductId == 2);
        Assert.Equal(1, p2.Quantity);
    }
}
