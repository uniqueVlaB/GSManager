using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services.Auth;
using GSManager.Core.Auth;
using GSManager.Core.Exceptions.Auth;
using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.Entities.Auth;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GSManager.Core.Services;

public partial class AuthService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtOptions> jwtOptions,
    IUnitOfWork unitOfWork,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthResult> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new InvalidCredentialsException();

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var rolePermissions = await (from role in _unitOfWork.IdentityRoles.GetQueryable()
                                     join roleClaim in _unitOfWork.IdentityRolesClaims.GetQueryable()
                                     on role.Id equals roleClaim.RoleId
                                     where roles.Contains(role.Name!) && roleClaim.ClaimType == CustomClaimTypes.Permission
                                     select roleClaim.ClaimValue).Distinct().ToListAsync(cancellationToken);

        var userClaims = await _userManager.GetClaimsAsync(user);
        var userPermissions = userClaims
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .Select(c => c.Value);

        var permissions = rolePermissions.Union(userPermissions).Distinct().ToList();

        var accessToken = GenerateJSONWebToken(user, roles, permissions);
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays),
            RememberMe = request.RememberMe ?? false
        };

        _unitOfWork.RefreshTokens.RemoveRange(_unitOfWork.RefreshTokens.GetQueryable().Where(rt => rt.UserId == user.Id));
        _unitOfWork.RefreshTokens.Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenEntity.Token,
            RememberMe = request.RememberMe ?? false
        };
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _unitOfWork.RefreshTokens.GetAsync(
            rt => rt.Token == refreshToken,
            cancellationToken,
            includeProperties: [nameof(RefreshToken.User)]);

        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAtUtc < DateTime.UtcNow)
        {
            throw new InvalidTokenException();
        }

        var roles = await _userManager.GetRolesAsync(refreshTokenEntity.User);

        var rolePermissions = await (from role in _unitOfWork.IdentityRoles.GetQueryable()
                                     join roleClaim in _unitOfWork.IdentityRolesClaims.GetQueryable()
                                     on role.Id equals roleClaim.RoleId
                                     where roles.Contains(role.Name!) && roleClaim.ClaimType == CustomClaimTypes.Permission
                                     select roleClaim.ClaimValue).Distinct().ToListAsync(cancellationToken);

        var userClaims = await _userManager.GetClaimsAsync(refreshTokenEntity.User);
        var userPermissions = userClaims
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .Select(c => c.Value);

        var rolesPermissions = rolePermissions.Union(userPermissions).Distinct().ToList();

        var accessToken = GenerateJSONWebToken(refreshTokenEntity.User, roles, rolesPermissions);

        var newRefreshToken = GenerateRefreshToken();

        await SaveNewRefreshTokenAsync(
            refreshTokenEntity,
            newRefreshToken,
            cancellationToken);

        return new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            RememberMe = refreshTokenEntity.RememberMe
        };

    }

    public async Task ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new UserNotFoundException();

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                LogEmailConfirmationFailure(userId, error.Code, error.Description);
            }

            throw new EmailConfirmationFailedException();
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Email confirmation failed for user {UserId}: [{Code}] {Description}")]
    private partial void LogEmailConfirmationFailure(Guid userId, string code, string description);

    private string GenerateJSONWebToken(ApplicationUser user, ICollection<string> roles, ICollection<string> permissions)
    {
        List<Claim> claims =
    [
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Name, user.UserName ?? user.Email!),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ..roles.Select(role => new Claim(ClaimTypes.Role, role)),
        ..permissions.Select(permission => new Claim(CustomClaimTypes.Permission, permission))
    ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    private async Task SaveNewRefreshTokenAsync(RefreshToken refreshTokenEntity, string newRefreshToken, CancellationToken cancellationToken)
    {
        _unitOfWork.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = refreshTokenEntity.UserId,
            Token = newRefreshToken,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays),
            RememberMe = refreshTokenEntity.RememberMe
        });
        _unitOfWork.RefreshTokens.Remove(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
