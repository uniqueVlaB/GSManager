using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthentificationController(
    IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        throw new NotImplementedException();
    }
}
