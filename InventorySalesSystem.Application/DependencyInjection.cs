using FluentValidation;
using InventorySalesSystem.Application.Services;
using InventorySalesSystem.Application.Services.Interfaces;
using InventorySalesSystem.Application.Validation.Sales;
using Microsoft.Extensions.DependencyInjection;
using InventorySalesSystem.Application.Events;

namespace InventorySalesSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddValidatorsFromAssemblyContaining<CreateSaleRequestValidator>();
        services.AddSingleton<SaleEventPublisher>();

        return services;
    }
}
