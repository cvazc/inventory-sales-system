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
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (product.Price <= 0)
        {
            return BadRequest("Product price must be greater than zero.");
        }

        var createdProduct = await _productService.CreateAsync(product);

        return CreatedAtAction(nameof(GetAll), new { id = createdProduct.Id }, createdProduct);
    }
}