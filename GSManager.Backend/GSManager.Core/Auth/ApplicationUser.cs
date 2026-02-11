using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Auth;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid MemberId { get; set; }
    public string? AvatarUrl { get; set; }
}
