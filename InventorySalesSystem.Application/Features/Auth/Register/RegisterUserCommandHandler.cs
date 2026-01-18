using InventorySalesSystem.Application.Abstractions.Persistence;
using InventorySalesSystem.Application.Abstractions.Security;
using InventorySalesSystem.Application.Contracts.Auth;
using InventorySalesSystem.Application.Exceptions;
using InventorySalesSystem.Domain.Entities;

namespace InventorySalesSystem.Application.Features.Auth.Register;

public sealed class RegisterUserCommandHandler
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public RegisterUserCommandHandler(IUserRepository users, IPasswordHasher hasher, ITokenService tokens)
    {
        _users = users;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<AuthResponse> HandleAsync(RegisterUserCommand command, CancellationToken ct = default)
    {
        var email = command.Request.Email.Trim().ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(email, ct);
        if (existing is not null)
            throw new BadRequestException("Email is already registered.");

        var role = string.IsNullOrWhiteSpace(command.Request.Role) ? "Clerk" : command.Request.Role.Trim();

        var user = new AppUser
        {
            Email = email,
            PasswordHash = _hasher.Hash(command.Request.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return new AuthResponse
        {
            Email = user.Email,
            Role = user.Role,
            AccessToken = _tokens.GenerateAccessToken(user)
        };
    }
}
