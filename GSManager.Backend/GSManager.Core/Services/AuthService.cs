using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Identity;
using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GSManager.Core.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<string?> LoginOrDefaultAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return null;
        }
        else
        {
            var roles = await _userManager.GetRolesAsync(user);
            return GenerateJSONWebToken(user.UserName ?? request.Email, roles);
        }
    }

    private string GenerateJSONWebToken(string userName, ICollection<string> roles)
    {
        List<Claim> claims =
        [
            new (JwtRegisteredClaimNames.Sub, userName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ..roles.Select(role => new Claim(ClaimTypes.Role, role))
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpirationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
