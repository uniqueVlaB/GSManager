using GSManager.Core.Auth;

namespace GSManager.Core.Models.Entities.Auth;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    //Navigation properties
    public ApplicationUser User { get; set; } = null!;

}
