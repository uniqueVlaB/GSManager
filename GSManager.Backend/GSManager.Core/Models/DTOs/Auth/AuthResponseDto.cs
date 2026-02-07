namespace GSManager.Core.Models.DTOs.Auth;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public required string UserId { get; set; }
    public Guid? MemberId { get; set; }
    public required string Email { get; set; }
    public string? Role { get; set; }
}
