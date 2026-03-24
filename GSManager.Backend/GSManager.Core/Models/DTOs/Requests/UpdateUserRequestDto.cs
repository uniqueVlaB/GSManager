namespace GSManager.Core.Models.DTOs.Requests;

public record UpdateUserRequestDto
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public Guid? MemberId { get; init; }
    public string? AvatarUrl { get; init; }
    public ICollection<Guid>? RoleIds { get; init; }
    public ICollection<string>? Permissions { get; init; }
}
