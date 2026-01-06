using InventorySalesSystem.Api.Models;
using InventorySalesSystem.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
