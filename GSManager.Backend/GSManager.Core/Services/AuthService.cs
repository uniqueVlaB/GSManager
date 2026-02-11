using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Auth;
using GSManager.Core.Exceptions.Auth;
using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Models.Entities.Auth;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GSManager.Core.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtOptions> jwtOptions,
    IUnitOfWork unitOfWork) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new InvalidCredentialsException();

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var rolesPermissions = await (from role in _unitOfWork.IdentityRoles.GetQueryable()
                                      join roleClaim in _unitOfWork.IdentityRolesClaims.GetQueryable()
                                      on role.Id equals roleClaim.RoleId
                                      where roles.Contains(role.Name!) && roleClaim.ClaimType == CustomClaimTypes.Permission
                                      select roleClaim.ClaimValue).Distinct().ToListAsync(cancellationToken);

        var accessToken = GenerateJSONWebToken(user.UserName ?? request.Email, roles, rolesPermissions);
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        _unitOfWork.RefreshTokens.RemoveRange(_unitOfWork.RefreshTokens.GetQueryable().Where(rt => rt.UserId == user.Id));
        _unitOfWork.RefreshTokens.Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenEntity.Token
        };

    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _unitOfWork.RefreshTokens.GetAsync(
            rt => rt.Token == refreshToken.Token,
            cancellationToken,
            includeProperties: [nameof(RefreshToken.User)]);

        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAtUtc < DateTime.UtcNow)
        {
            throw new InvalidTokenException();
        }

        var roles = await _userManager.GetRolesAsync(refreshTokenEntity.User);

        var rolesPermissions = await (from role in _unitOfWork.IdentityRoles.GetQueryable()
                                      join roleClaim in _unitOfWork.IdentityRolesClaims.GetQueryable()
                                      on role.Id equals roleClaim.RoleId
                                      where roles.Contains(role.Name!) && roleClaim.ClaimType == CustomClaimTypes.Permission
                                      select roleClaim.ClaimValue).Distinct().ToListAsync(cancellationToken);

        var accessToken = GenerateJSONWebToken(refreshTokenEntity.User.UserName ?? refreshTokenEntity.User.Email!, roles, rolesPermissions);

        var newRefreshToken = GenerateRefreshToken();
        _unitOfWork.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = refreshTokenEntity.UserId,
            Token = newRefreshToken,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        });
        _unitOfWork.RefreshTokens.Remove(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };

    }

    private string GenerateJSONWebToken(string userName, ICollection<string> roles, ICollection<string> permissions)
    {
        List<Claim> claims =
        [
            new (JwtRegisteredClaimNames.Sub, userName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ..roles.Select(role => new Claim(ClaimTypes.Role, role)),
            ..permissions.Select(permission => new Claim(CustomClaimTypes.Permission, permission))
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
