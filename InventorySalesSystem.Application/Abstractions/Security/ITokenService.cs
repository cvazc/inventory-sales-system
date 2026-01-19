using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Abstractions.Security;

public interface ITokenService
{
    string GenerateAccessToken(AppUser user);
}