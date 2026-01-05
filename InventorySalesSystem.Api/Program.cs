using InventorySalesSystem.Api.Models;
using InventorySalesSystem.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var productService = new ProductService();

app.MapGet("/products", () =>
{
   var products = productService.GetAll();
   return Results.Ok(products);
});

app.MapPost("/products", (Product product) =>
{
    var createdProduct = productService.Create(product);
    return Results.Created($"/products/{createdProduct.Id}", createdProduct);
});


app.Run();
