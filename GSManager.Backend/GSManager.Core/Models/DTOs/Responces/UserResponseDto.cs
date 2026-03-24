namespace GSManager.Core.Models.DTOs.Responces;

public class UserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public Guid? MemberId { get; set; }
    public string? AvatarUrl { get; set; }
    public ICollection<string>? Roles { get; init; }
    public ICollection<string>? Permissions { get; init; }
}
