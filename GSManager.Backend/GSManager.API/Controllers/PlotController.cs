using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.API.Controllers;

[ApiController]
[Route("plots")]
public class PlotController(IPlotService plotService) : ControllerBase
{
    private readonly IPlotService _plotService = plotService;

    [HttpGet]
    public async Task<IActionResult> GetPlotsAsync(
        [FromQuery] PlotFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _plotService.GetPlotsAsync(filterDto, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("select-list")]
    public async Task<IActionResult> GetPlotSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _plotService.GetPlotSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{plotId:guid}")]
    public async Task<IActionResult> GetPlotByIdAsync(Guid plotId, CancellationToken cancellationToken)
    {
        var plot = await _plotService.GetPlotByIdAsync(plotId, cancellationToken);
        return Ok(plot);
    }

    [HttpPost]
    public async Task<IActionResult> AddPlotAsync([FromBody] PlotDto plotDto, CancellationToken cancellationToken)
    {
        var createdPlot = await _plotService.AddPlotAsync(plotDto, cancellationToken);
        return CreatedAtAction(nameof(GetPlotByIdAsync), new { plotId = createdPlot.Id }, createdPlot);
    }

    [HttpPut("{plotId:guid}")]
    public async Task<IActionResult> UpdatePlotAsync(Guid plotId, [FromBody] PlotDto plotDto, CancellationToken cancellationToken)
    {
        var updatedPlot = await _plotService.UpdatePlotAsync(plotId, plotDto, cancellationToken);
        return Ok(updatedPlot);
    }

    [HttpDelete("{plotId:guid}")]
    public async Task<IActionResult> DeletePlotAsync(Guid plotId, CancellationToken cancellationToken)
    {
        await _plotService.DeletePlotAsync(plotId, cancellationToken);
        return NoContent();
    }
}
