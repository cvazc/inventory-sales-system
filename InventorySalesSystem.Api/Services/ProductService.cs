using InventorySalesSystem.Api.Models;

namespace InventorySalesSystem.Api.Services;

public class ProductService
{
    private readonly List<Product> _products = new();

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product Create(Product product)
    {
        product.Id = _products.Count + 1;
        _products.Add(product);
        return product;
    }
}