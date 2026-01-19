using InventorySalesSystem.Application.Contracts.Sales;
using Microsoft.AspNetCore.Mvc;
using InventorySalesSystem.Application.Features.Sales.Create;
using InventorySalesSystem.Application.Features.Sales.GetById;
using InventorySalesSystem.Application.Features.Sales.GetPaged;
using Microsoft.AspNetCore.Authorization;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly GetSalesPagedQueryHandler _getPaged;
    private readonly CreateSaleCommandHandler _create;
    private readonly GetSaleByIdQueryHandler _getById;

    public SalesController(
        GetSalesPagedQueryHandler getPaged,
        CreateSaleCommandHandler create,
        GetSaleByIdQueryHandler getById)
    {
        _getPaged = getPaged;
        _create = create;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var sales = await _getPaged.HandleAsync(new GetSalesPagedQuery(page, pageSize));
        return Ok(sales);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleRequest request)
    {
        var sale = await _create.HandleAsync(new CreateSaleCommand(request));
        return Created($"/api/sales/{sale.Id}", sale);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _getById.HandleAsync(new GetSaleByIdQuery(id));
        return Ok(sale);
    }
}