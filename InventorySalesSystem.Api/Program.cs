using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Middleware;
using InventorySalesSystem.Api.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SaleService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
