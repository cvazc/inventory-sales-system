using InventorySalesSystem.Api.Contracts.Sales;
using InventorySalesSystem.Api.Services;
using InventorySalesSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var sales = await _saleService.GetAllAsync(page, pageSize);
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