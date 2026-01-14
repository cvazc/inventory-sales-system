using FluentValidation;
using InventorySalesSystem.Application.Services;
using InventorySalesSystem.Application.Services.Interfaces;
using InventorySalesSystem.Application.Validation.Sales;
using Microsoft.Extensions.DependencyInjection;
using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Application.Features.Sales.Create;
using InventorySalesSystem.Application.Features.Sales.GetById;
using InventorySalesSystem.Application.Features.Sales.GetPaged;

namespace InventorySalesSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssemblyContaining<CreateSaleRequestValidator>();
        services.AddSingleton<SaleEventPublisher>();
        services.AddScoped<CreateSaleCommandHandler>();
        services.AddScoped<GetSaleByIdQueryHandler>();
        services.AddScoped<GetSalesPagedQueryHandler>();


        return services;
    }
}
