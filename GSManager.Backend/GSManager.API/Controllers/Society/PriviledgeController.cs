using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities.Society;
using GSManager.Core.Models.DTOs.Filters.Society;
using Microsoft.AspNetCore.Authorization;
using GSManager.Core.Abstractions.Services.Society;

namespace GSManager.API.Controllers.Society;

[ApiController]
[Route("api/priviledges")]
[Authorize]
[Tags("Priviledges")]
public class PriviledgeController(IPriviledgeService priviledgeService) : ControllerBase
{
    private readonly IPriviledgeService _priviledgeService = priviledgeService;

    [HttpGet]
    [EndpointSummary("Get priviledges")]
    [EndpointDescription("Returns all priviledges, or a filtered subset when query parameters are provided.")]
    [ProducesResponseType<ICollection<PriviledgeDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
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

    [HttpGet("select-list")]
    [EndpointSummary("Get priviledge select list")]
    [ProducesResponseType<ICollection<SelectListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPriviledgeSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _priviledgeService.GetPriviledgeSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{priviledgeId:guid}")]
    [EndpointSummary("Get priviledge by ID")]
    [ProducesResponseType<PriviledgeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPriviledgeByIdAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        var priviledge = await _priviledgeService.GetPriviledgeByIdAsync(priviledgeId, cancellationToken);
        return Ok(priviledge);
    }

    [HttpPost]
    [EndpointSummary("Create priviledge")]
    [ProducesResponseType<PriviledgeDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPriviledgeAsync([FromBody] PriviledgeDto priviledgeDto, CancellationToken cancellationToken)
    {
        var createdPriviledge = await _priviledgeService.AddPriviledgeAsync(priviledgeDto, cancellationToken);
        return CreatedAtAction(nameof(GetPriviledgeByIdAsync), new { priviledgeId = createdPriviledge.Id }, createdPriviledge);
    }

    [HttpPut("{priviledgeId:guid}")]
    [EndpointSummary("Update priviledge")]
    [ProducesResponseType<PriviledgeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePriviledgeAsync(
        Guid priviledgeId,
        [FromBody] PriviledgeDto priviledgeDto,
        CancellationToken cancellationToken)
    {
        var updatedPriviledge = await _priviledgeService.UpdatePriviledgeAsync(priviledgeId, priviledgeDto, cancellationToken);
        return Ok(updatedPriviledge);
    }

    [HttpDelete("{priviledgeId:guid}")]
    [EndpointSummary("Delete priviledge")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePriviledgeAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        await _priviledgeService.DeletePriviledgeAsync(priviledgeId, cancellationToken);
        return NoContent();
    }
}
