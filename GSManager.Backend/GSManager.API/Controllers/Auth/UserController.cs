using GSManager.Core.Abstractions.Services.Auth;
using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/users")]
[Authorize]
[Tags("Users")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("me")]
    [EndpointSummary("Get current user")]
    [ProducesResponseType<UserResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserAsync(
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetCurrentUserDtoAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = Permissions.ViewUsers)]
    [EndpointSummary("Get user by ID")]
    [ProducesResponseType<UserResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(
        Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.ViewUsers)]
    [EndpointSummary("Get users (paged)")]
    [ProducesResponseType<PagedResultDto<UserResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsersAsync(
        [FromQuery] PagedRequestDto pagedRequest, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUsersAsync(pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("permissions")]
    [Authorize(Policy = Permissions.ViewUsers)]
    [EndpointSummary("Get all available permissions")]
    [ProducesResponseType<ICollection<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        return Ok(Permissions.GetAllPermissions());
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddUsers)]
    [EndpointSummary("Create user")]
    [ProducesResponseType<UserResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUserAsync(
        [FromBody] CreateUserRequestDto createUserDto, CancellationToken cancellationToken)
    {
        var createdUser = await _userService.CreateUserAsync(createUserDto, cancellationToken);
        return Ok(createdUser);
    }

    [HttpDelete("{userId:guid}")]
    [Authorize(Policy = Permissions.DeleteUsers)]
    [EndpointSummary("Delete user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAsync(
        Guid userId, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{userId:guid}")]
    [Authorize(Policy = Permissions.EditUsers)]
    [EndpointSummary("Update user")]
    [ProducesResponseType<UserResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserAsync(
        Guid userId, [FromBody] UpdateUserRequestDto updateUserDto, CancellationToken cancellationToken)
    {
        var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDto, cancellationToken);
        return Ok(updatedUser);
    }
}
