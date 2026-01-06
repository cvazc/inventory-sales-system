using InventorySalesSystem.Api.Models;
using InventorySalesSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (product.Price <= 0)
        {
            return BadRequest("Product price must be greater than zero.");
        }

        var createdProduct = _productService.Create(product);

        return CreatedAtAction(nameof(GetAll), new { id = createdProduct.Id }, createdProduct);
    }
}