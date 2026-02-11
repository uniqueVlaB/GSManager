using GSManager.Core.Models.DTOs.Auth;
using GSManager.Core.Models.Entities.Auth;

namespace GSManager.Core.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
    Task<AuthResponseDto> RefreshAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}
