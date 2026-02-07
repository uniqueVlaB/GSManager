using GSManager.Core.Models.DTOs.Auth;

namespace GSManager.Core.Abstractions.Services;

public interface IAuthService
{
    Task<string?> LoginOrDefaultAsync(LoginRequestDto request, CancellationToken cancellationToken);
}
