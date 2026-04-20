using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities.Society;
using GSManager.Core.Models.DTOs.Filters.Society;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Authorization;
using GSManager.Core.Auth;
using GSManager.Core.Abstractions.Services.Society;

namespace GSManager.API.Controllers.Society;

[ApiController]
[Route("api/plots")]
[Authorize]
[Tags("Plots")]
public class PlotController(IPlotService plotService) : ControllerBase
{
    private readonly IPlotService _plotService = plotService;

    [HttpGet]
    [Authorize(Policy = Permissions.ViewPlots)]
    [EndpointSummary("Get plots (paged)")]
    [ProducesResponseType<PagedResultDto<PlotDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPlotsAsync(
        [FromQuery] PlotFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _plotService.GetPlotsAsync(filterDto, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("select-list")]
    [Authorize(Policy = Permissions.ViewPlots)]
    [EndpointSummary("Get plot select list")]
    [ProducesResponseType<ICollection<SelectListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPlotSelectListAsync(
        CancellationToken cancellationToken)
    {
        var selectList = await _plotService.GetPlotSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{plotId:guid}")]
    [Authorize(Policy = Permissions.ViewPlots)]
    [EndpointSummary("Get plot by ID")]
    [ProducesResponseType<PlotDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlotByIdAsync(
        Guid plotId, CancellationToken cancellationToken)
    {
        var plot = await _plotService.GetPlotByIdAsync(plotId, cancellationToken);
        return Ok(plot);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddPlots)]
    [EndpointSummary("Create plot")]
    [ProducesResponseType<PlotDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddPlotAsync(
        [FromBody] PlotDto plotDto, CancellationToken cancellationToken)
    {
        var createdPlot = await _plotService.AddPlotAsync(plotDto, cancellationToken);
        return CreatedAtAction(nameof(GetPlotByIdAsync), new { plotId = createdPlot.Id }, createdPlot);
    }

    [HttpPut("{plotId:guid}")]
    [Authorize(Policy = Permissions.EditPlots)]
    [EndpointSummary("Update plot")]
    [ProducesResponseType<PlotDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePlotAsync(
        Guid plotId, [FromBody] PlotDto plotDto, CancellationToken cancellationToken)
    {
        var updatedPlot = await _plotService.UpdatePlotAsync(plotId, plotDto, cancellationToken);
        return Ok(updatedPlot);
    }

    [HttpDelete("{plotId:guid}")]
    [Authorize(Policy = Permissions.DeletePlots)]
    [EndpointSummary("Delete plot")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePlotAsync(
        Guid plotId, CancellationToken cancellationToken)
    {
        await _plotService.DeletePlotAsync(plotId, cancellationToken);
        return NoContent();
    }
}
