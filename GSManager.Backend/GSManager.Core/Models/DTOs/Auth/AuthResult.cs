namespace GSManager.Core.Models.DTOs.Auth;

/// <summary>
/// Internal result containing both access token and refresh token.
/// The refresh token should be set as an HttpOnly cookie and not exposed in the response.
/// </summary>
public class AuthResult
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public bool RememberMe { get; set; }
}
