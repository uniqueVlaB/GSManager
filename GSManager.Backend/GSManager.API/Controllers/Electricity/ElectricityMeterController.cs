using GSManager.Core.Abstractions.Services.Electricity;
using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Entities.Electricity;
using GSManager.Core.Models.DTOs.Filters.Electricity;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Electricity;

[Route("api/electricity-meters")]
[ApiController]
[Authorize]
[Tags("Electricity Meters")]
public class ElectricityMeterController(IElectricityMeterService electricityMeterService) : ControllerBase
{
    private readonly IElectricityMeterService _electricityMeterService = electricityMeterService;

    [HttpGet]
    [Authorize(Policy = Permissions.ViewElectricityMeters)]
    [EndpointSummary("Get electricity meters (paged)")]
    [ProducesResponseType<PagedResultDto<ElectricityMeterDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetElectricityMetersAsync(
        [FromQuery] ElectricityMeterFilterDto filter,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.GetElectricityMetersAsync(filter, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.ViewElectricityMeters)]
    [EndpointSummary("Get electricity meter by ID")]
    [ProducesResponseType<ElectricityMeterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetElectricityMeterByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.GetElectricityMeterByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddElectricityMeters)]
    [EndpointSummary("Add electricity meter")]
    [ProducesResponseType<ElectricityMeterDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddElectricityMeterAsync([FromBody] ElectricityMeterDto electricityMeterDto, CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.AddElectricityMeterAsync(electricityMeterDto, cancellationToken);
        return CreatedAtAction(nameof(GetElectricityMeterByIdAsync), new { id = result.Id }, result);
    }

    //[HttpDelete("{id:guid}")]
    //[Authorize(Policy = Permissions.DeleteElectricityMeters)]
    //public async Task<IActionResult> DeleteElectricityMeterAsync(Guid id, CancellationToken cancellationToken)
    //{
    //    await _electricityMeterService.DeleteElectricityMeterAsync(id, cancellationToken);
    //    return NoContent();
    //}
}
