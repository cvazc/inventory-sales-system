using InventorySalesSystem.Api.Contracts.Common;
using InventorySalesSystem.Api.Contracts.Sales;

namespace InventorySalesSystem.Api.Services.Interfaces;

public interface ISaleService
{
    Task<PagedResult<SaleResponse>> GetAllAsync(int page, int pageSize);
    Task<SaleResponse> GetByIdAsync(int id);
    Task<SaleResponse> CreateAsync(CreateSaleRequest request);
}
