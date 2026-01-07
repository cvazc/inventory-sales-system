using InventorySalesSystem.Api.Data;
using InventorySalesSystem.Api.Services;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
