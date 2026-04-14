using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services.Auth;

public interface IUserService
{
    Task<UserResponseDto> GetCurrentUserDtoAsync(CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<PagedResultDto<UserResponseDto>> GetUsersAsync(PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto createUserDto, CancellationToken cancellationToken);
    Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto, CancellationToken cancellationToken);
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
}
