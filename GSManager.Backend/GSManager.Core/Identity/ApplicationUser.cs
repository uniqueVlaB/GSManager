using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Identity;

public class ApplicationUser : IdentityUser
{
    public Guid MemberId { get; set; }
    public string? AvatarUrl { get; set; }
}
