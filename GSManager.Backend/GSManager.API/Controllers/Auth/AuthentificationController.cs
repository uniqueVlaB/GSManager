using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthentificationController(
    IAuthService authService, IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken, request.RememberMe);

        return Ok(new AuthResponseDto { AccessToken = result.AccessToken });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAsync(CancellationToken cancellationToken)
    {
        // Read refresh token from HttpOnly cookie
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token not found" });
        }

        var result = await _authService.RefreshAsync(refreshToken, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken, result.RememberMe);

        return Ok(new AuthResponseDto { AccessToken = result.AccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Delete("refreshToken", cookieOptions);
        return Ok();
    }

    private void SetRefreshTokenCookie(string token, bool? rememberMe = null)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = rememberMe == true ? DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays) : null,
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
