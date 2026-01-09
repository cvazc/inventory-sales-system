using InventorySalesSystem.Api.Contracts.Sales;
using InventorySalesSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly SaleService _saleService;

    public SalesController(SaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleService.GetAllAsync();
        return Ok(sales);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleRequest request)
    {
        var sale = await _saleService.CreateAsync(request);
        return Created($"/api/sales/{sale.Id}", sale);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        return Ok(sale);
    }
}