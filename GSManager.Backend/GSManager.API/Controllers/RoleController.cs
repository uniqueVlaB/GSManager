using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.API.Controllers;

[ApiController]
[Route("roles")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> GetRolesAsync(
        [FromQuery] RoleFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRolesAsync(filterDto, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("select-list")]
    public async Task<IActionResult> GetRoleSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _roleService.GetRoleSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{roleId:guid}")]
    public async Task<IActionResult> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId, cancellationToken);
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto roleDto, CancellationToken cancellationToken)
    {
        var createdRole = await _roleService.AddRoleAsync(roleDto, cancellationToken);
        return CreatedAtAction(nameof(GetRoleByIdAsync), new { roleId = createdRole.Id }, createdRole);
    }

    [HttpPut("{roleId:guid}")]
    public async Task<IActionResult> UpdateRoleAsync(Guid roleId, [FromBody] RoleDto roleDto, CancellationToken cancellationToken)
    {
        var updatedRole = await _roleService.UpdateRoleAsync(roleId, roleDto, cancellationToken);
        return Ok(updatedRole);
    }

    [HttpDelete("{roleId:guid}")]
    public async Task<IActionResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        await _roleService.DeleteRoleAsync(roleId, cancellationToken);
        return NoContent();
    }
}
