using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities.Society;
using GSManager.Core.Models.DTOs.Filters.Society;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Authorization;
using GSManager.Core.Abstractions.Services.Society;

namespace GSManager.API.Controllers.Society;

[ApiController]
[Route("api/roles")]
[Authorize]
[Tags("Roles")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    [EndpointSummary("Get roles (paged)")]
    [ProducesResponseType<PagedResultDto<RoleDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRolesAsync(
        [FromQuery] RoleFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRolesAsync(filterDto, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("select-list")]
    [EndpointSummary("Get role select list")]
    [ProducesResponseType<ICollection<SelectListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoleSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _roleService.GetRoleSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{roleId:guid}")]
    [EndpointSummary("Get role by ID")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId, cancellationToken);
        return Ok(role);
    }

    [HttpPost]
    [EndpointSummary("Create role")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto roleDto, CancellationToken cancellationToken)
    {
        var createdRole = await _roleService.AddRoleAsync(roleDto, cancellationToken);
        return Ok(createdRole);
    }

    [HttpPut("{roleId:guid}")]
    [EndpointSummary("Update role")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoleAsync(Guid roleId, [FromBody] RoleDto roleDto, CancellationToken cancellationToken)
    {
        var updatedRole = await _roleService.UpdateRoleAsync(roleId, roleDto, cancellationToken);
        return Ok(updatedRole);
    }

    [HttpDelete("{roleId:guid}")]
    [EndpointSummary("Delete role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        await _roleService.DeleteRoleAsync(roleId, cancellationToken);
        return NoContent();
    }
}
