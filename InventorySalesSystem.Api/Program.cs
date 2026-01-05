using InventorySalesSystem.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<Product> products = new List<Product>();

app.MapGet("/products", () =>
{
   return Results.Ok(products); 
});

app.MapPost("/products", (Product product) =>
{
    product.Id = products.Count + 1;

    products.Add(product);

    return Results.Created($"/products/{product.Id}", product);
});


app.Run();
