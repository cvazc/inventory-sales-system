using InventorySalesSystem.Api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Infrastructure.Handlers;
using InventorySalesSystem.Infrastructure;
using InventorySalesSystem.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<SaleEventPublisher>();
builder.Services.AddSingleton<SaleAuditLogHandler>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        
    });

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

var salePublisher = app.Services.GetRequiredService<SaleEventPublisher>();
var auditHandler = app.Services.GetRequiredService<SaleAuditLogHandler>();

salePublisher.SaleCreated += auditHandler.OnSaleCreated;

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
