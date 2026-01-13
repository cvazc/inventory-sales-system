using InventorySalesSystem.Application.Contracts.Common;
using InventorySalesSystem.Application.Contracts.Sales;

namespace InventorySalesSystem.Application.Services.Interfaces;

public interface ISaleService
{
    Task<PagedResult<SaleResponse>> GetAllAsync(int page, int pageSize);
    Task<SaleResponse> GetByIdAsync(int id);
    Task<SaleResponse> CreateAsync(CreateSaleRequest request);
}
