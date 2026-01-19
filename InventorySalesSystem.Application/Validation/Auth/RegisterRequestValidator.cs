using FluentValidation;
using InventorySalesSystem.Application.Contracts.Auth;

namespace InventorySalesSystem.Application.Validation.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.Role).MaximumLength(50);
    }
}