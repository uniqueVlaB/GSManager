namespace GSManager.Core.Models.DTOs.Requests;

public record CreateUserRequestDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public Guid? MemberId { get; init; }
    public string? AvatarUrl { get; init; }
    public ICollection<Guid>? RoleIds { get; init; }
    public ICollection<string>? Permissions { get; init; }
}
