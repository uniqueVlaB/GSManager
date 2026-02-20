using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.Core.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
    Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
}
