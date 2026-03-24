using GSManager.Core.Abstractions.Services;
using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var result = await _userService.GetCurrentUserDtoAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = Permissions.ViewUsers)]
    public async Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.ViewUsers)]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] PagedRequestDto pagedRequest, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllUsersAsync(pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("permissions")]
    [Authorize(Policy = Permissions.ViewUsers)]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        return Ok(Permissions.GetAllPermissions());
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddUsers)]
    public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto createUserDto, CancellationToken cancellationToken)
    {
        var createdUser = await _userService.CreateUserAsync(createUserDto, cancellationToken);
        return Ok(createdUser);
    }

    [HttpDelete("{userId:guid}")]
    [Authorize(Policy = Permissions.DeleteUsers)]
    public async Task<IActionResult> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{userId:guid}")]
    [Authorize(Policy = Permissions.EditUsers)]
    public async Task<IActionResult> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto, CancellationToken cancellationToken)
    {
        var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDto, cancellationToken);
        return Ok(updatedUser);
    }
}
