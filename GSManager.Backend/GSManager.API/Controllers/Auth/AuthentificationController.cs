using GSManager.Core.Abstractions.Services.Auth;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[Tags("Authentication")]
public class AuthentificationController(
    IAuthService authService, IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("login")]
    [EndpointSummary("Login")]
    [EndpointDescription("Authenticates the user and returns an access token. The refresh token is set as an HttpOnly cookie.")]
    [ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken, request.RememberMe);

        return Ok(new AuthResponseDto { AccessToken = result.AccessToken });
    }

    //[HttpPost("register")]
    //public async Task<IActionResult> RegisterAsync(CancellationToken cancellationToken)
    //{
    //    _mailer.SendEmailConfirmation("bulahvlad7@gmail.com", "UserName", Guid.NewGuid(), "Token", cancellationToken);
    //    return Ok();
    //}

    [HttpPost("confirm-email")]
    [EndpointSummary("Confirm email")]
    [EndpointDescription("Confirms the user's email address using the token sent after registration.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromQuery] Guid userId, [FromQuery] string token, CancellationToken cancellationToken)
    {
        await _authService.ConfirmEmailAsync(userId, token, cancellationToken);
        return Ok();
    }

    [HttpPost("refresh-token")]
    [EndpointSummary("Refresh access token")]
    [EndpointDescription("Issues a new access token using the refresh token stored in the HttpOnly cookie.")]
    [ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshAsync(
        CancellationToken cancellationToken)
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
    [EndpointSummary("Logout")]
    [EndpointDescription("Clears the refresh token cookie and ends the current session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
