using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.Core.Abstractions.Services.Auth;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
    Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    Task ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken);
}
