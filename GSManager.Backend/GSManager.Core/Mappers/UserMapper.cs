using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToUserResponseDto(ApplicationUser user, ICollection<string>? roles, ICollection<string>? permissions)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            MemberId = user.MemberId,
            AvatarUrl = user.AvatarUrl,
            Roles = roles,
            Permissions = permissions
        };
    }
}
