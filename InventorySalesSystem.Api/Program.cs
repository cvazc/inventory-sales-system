using InventorySalesSystem.Api.Models;
using InventorySalesSystem.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

app.MapGet("/products", (ProductService productService) =>
{
   var products = productService.GetAll();
   return Results.Ok(products);
});

app.MapPost("/products", (ProductService productService, Product product) =>
{
    var createdProduct = productService.Create(product);
    return Results.Created($"/products/{createdProduct.Id}", createdProduct);
});


app.Run();
