using InventorySalesSystem.Api.Contracts.Products;
using InventorySalesSystem.Api.Models;
using InventorySalesSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using InventorySalesSystem.Api.Services.Interfaces;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
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
        if (string.IsNullOrWhiteSpace(product.Sku))
        {
            return BadRequest("Product SKU is required.");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (product.Price <= 0)
        {
            return BadRequest("Product price must be greater than zero.");
        }

        if (product.StockQuantity < 0)
        {
            return BadRequest("Stock quantity cannot be negative.");
        }

        var createdProduct = await _productService.CreateAsync(product);

        return CreatedAtAction(nameof(GetAll), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:int}/stock")]
    public async Task<IActionResult> AdjustStock(int id, AdjustStockRequest request)
    {
        var updatedProduct = await _productService.AdjustStockAsync(id, request.Delta);
        return Ok(updatedProduct);
    }
}