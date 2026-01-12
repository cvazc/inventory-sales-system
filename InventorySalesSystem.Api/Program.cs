using InventorySalesSystem.Infrastructure.Persistence;
using InventorySalesSystem.Api.Middleware;
using InventorySalesSystem.Api.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using InventorySalesSystem.Api.Services.Interfaces;
using InventorySalesSystem.Api.Events;
using InventorySalesSystem.Api.Events.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql =>
        sql.MigrationsAssembly("InventorySalesSystem.Infrastructure"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddSingleton<SaleEventPublisher>();
builder.Services.AddSingleton<SaleAuditLogHandler>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

var salePublisher = app.Services.GetRequiredService<SaleEventPublisher>();
var auditHandler = app.Services.GetRequiredService<SaleAuditLogHandler>();

salePublisher.SaleCreated += auditHandler.OnSaleCreated;

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
