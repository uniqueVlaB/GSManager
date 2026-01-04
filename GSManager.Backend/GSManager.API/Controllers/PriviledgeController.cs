using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs;

namespace GSManager.API.Controllers;

[ApiController]
[Route("priviledges")]
public class PriviledgeController(IPriviledgeService priviledgeService) : ControllerBase
{
    private readonly IPriviledgeService _priviledgeService = priviledgeService;

    [HttpGet]
    public async Task<IActionResult> GetFilteredPriviledgesAsync(
        [FromQuery] PriviledgeFilterDto? filterDto,
        CancellationToken cancellationToken)
    {
        ICollection<PriviledgeDto>? dtos;
        if (filterDto is null)
        {
            dtos = await _priviledgeService.GetAllPriviledgesAsync(cancellationToken);
        }
        else
        {
            dtos = await _priviledgeService.GetFilteredPriviledgesAsync(filterDto, cancellationToken);
        }

        return Ok(dtos);
    }

    [HttpGet("{priviledgeId:guid}")]
    public async Task<IActionResult> GetPriviledgeByIdAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        var priviledge = await _priviledgeService.GetPriviledgeByIdAsync(priviledgeId, cancellationToken);
        return Ok(priviledge);
    }

    [HttpPost]
    public async Task<IActionResult> AddPriviledgeAsync([FromBody] PriviledgeDto priviledgeDto, CancellationToken cancellationToken)
    {
        var createdPriviledge = await _priviledgeService.AddPriviledgeAsync(priviledgeDto, cancellationToken);
        return CreatedAtAction(nameof(GetPriviledgeByIdAsync), new { priviledgeId = createdPriviledge.Id }, createdPriviledge);
    }

    [HttpPut("{priviledgeId:guid}")]
    public async Task<IActionResult> UpdatePriviledgeAsync(
        Guid priviledgeId,
        [FromBody] PriviledgeDto priviledgeDto,
        CancellationToken cancellationToken)
    {
        var updatedPriviledge = await _priviledgeService.UpdatePriviledgeAsync(priviledgeId, priviledgeDto, cancellationToken);
        return Ok(updatedPriviledge);
    }

    [HttpDelete("{priviledgeId:guid}")]
    public async Task<IActionResult> DeletePriviledgeAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        await _priviledgeService.DeletePriviledgeAsync(priviledgeId, cancellationToken);
        return NoContent();
    }
}
