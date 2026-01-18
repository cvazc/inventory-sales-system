using InventorySalesSystem.Application.Contracts.Auth;

namespace InventorySalesSystem.Application.Features.Auth.Register;

public sealed class RegisterUserCommand
{
    public RegisterRequest Request { get; }

    public RegisterUserCommand(RegisterRequest request)
    {
        Request = request;
    }
}
