using InventorySalesSystem.Api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using InventorySalesSystem.Application.Events;
using InventorySalesSystem.Infrastructure.Handlers;
using InventorySalesSystem.Infrastructure;
using InventorySalesSystem.Application;
using System.Text;
using InventorySalesSystem.Api.Security;
using InventorySalesSystem.Application.Abstractions.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddSingleton<SaleAuditLogHandler>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {

    });

builder.Services.AddFluentValidationAutoValidation();

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var seedEnabled = config["Seed:Enabled"];

    if (string.Equals(seedEnabled, "true", StringComparison.OrdinalIgnoreCase))
    {
        var seeder = scope.ServiceProvider
            .GetRequiredService<InventorySalesSystem.Infrastructure.Persistence.Seeding.DatabaseSeeder>();

        await seeder.SeedAsync();
    }
}

var salePublisher = app.Services.GetRequiredService<SaleEventPublisher>();
var auditHandler = app.Services.GetRequiredService<SaleAuditLogHandler>();

salePublisher.SaleCreated += auditHandler.OnSaleCreated;

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
