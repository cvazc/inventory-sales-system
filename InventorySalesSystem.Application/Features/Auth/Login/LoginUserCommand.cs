using InventorySalesSystem.Application.Contracts.Auth;

namespace InventorySalesSystem.Application.Features.Auth.Login;

public sealed class LoginUserCommand
{
    public LoginRequest Request { get; }

    public LoginUserCommand(LoginRequest request)
    {
        Request = request;
    }
}
