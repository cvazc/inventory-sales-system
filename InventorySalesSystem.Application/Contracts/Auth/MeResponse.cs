namespace InventorySalesSystem.Application.Contracts.Auth;

public sealed class MeResponse
{
    public int UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
