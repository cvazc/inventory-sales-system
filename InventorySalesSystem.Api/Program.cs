using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseInMemoryDatabase("InventorySalesDatabase");
});

builder.Services.AddScoped<ProductService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
