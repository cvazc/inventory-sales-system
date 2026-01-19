using InventorySalesSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Infrastructure.Persistence.Repositories;

namespace InventorySalesSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly("InventorySalesSystem.Infrastructure"));
        });

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
