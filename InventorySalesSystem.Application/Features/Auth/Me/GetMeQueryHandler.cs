using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Contracts.Auth;

namespace InventorySalesSystem.Application.Features.Auth.Me;

public sealed class GetMeQueryHandler
{
    private readonly IUserRepository _users;

    public GetMeQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<MeResponse?> HandleAsync(string email, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(email, ct);
        if (user is null) return null;

        return new MeResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }
}