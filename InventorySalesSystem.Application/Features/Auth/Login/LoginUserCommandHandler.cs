using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Abstractions.Security;
using InventorySalesSystem.Application.Contracts.Auth;
using InventorySalesSystem.Application.Exceptions;

namespace InventorySalesSystem.Application.Features.Auth.Login;

public sealed class LoginUserCommandHandler
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public LoginUserCommandHandler(IUserRepository users, IPasswordHasher hasher, ITokenService tokens)
    {
        _users = users;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<AuthResponse> HandleAsync(LoginUserCommand command, CancellationToken ct = default)
    {
        var email = command.Request.Email.Trim().ToLowerInvariant();

        var user = await _users.GetByEmailAsync(email, ct);

        if (user is null || !_hasher.Verify(command.Request.Password, user.PasswordHash))
            throw new BadRequestException("Invalid credentials.");

        return new AuthResponse
        {
            Email = user.Email,
            Role = user.Role,
            AccessToken = _tokens.GenerateAccessToken(user)
        };
    }
}
