using System.Security.Claims;
using InventorySalesSystem.Application.Contracts.Auth;
using InventorySalesSystem.Application.Features.Auth.Login;
using InventorySalesSystem.Application.Features.Auth.Me;
using InventorySalesSystem.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySalesSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserCommandHandler _register;
    private readonly LoginUserCommandHandler _login;

    public AuthController(RegisterUserCommandHandler register, LoginUserCommandHandler login)
    {
        _register = register;
        _login = login;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
    {
        var result = await _register.HandleAsync(new RegisterUserCommand(request), ct);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _login.HandleAsync(new LoginUserCommand(request), ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me([FromServices] GetMeQueryHandler handler, CancellationToken ct)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return Unauthorized();

        var result = await handler.HandleAsync(email, ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

}
