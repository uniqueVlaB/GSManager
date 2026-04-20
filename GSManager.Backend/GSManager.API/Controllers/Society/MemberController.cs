using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities.Society;
using GSManager.Core.Models.DTOs.Filters.Society;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using GSManager.API.Telemetry;
using Microsoft.AspNetCore.Authorization;
using GSManager.Core.Auth;
using GSManager.Core.Abstractions.Services.Society;

namespace GSManager.API.Controllers.Society;

[ApiController]
[Route("api/members")]
[Authorize]
[Tags("Members")]
public class MemberController(IMemberService memberService, ApiMeters metrics) : ControllerBase
{
    private readonly IMemberService _memberService = memberService;
    private readonly ApiMeters _metrics = metrics;

    [HttpGet]
    [Authorize(Policy = Permissions.ViewMembers)]
    [EndpointSummary("Get members (paged)")]
    [ProducesResponseType<PagedResultDto<MemberDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMembersAsync(
        [FromQuery] MemberFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMembersAsync(filterDto, pagedRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("select-list")]
    [Authorize(Policy = Permissions.ViewMembers)]
    [EndpointSummary("Get member select list")]
    [ProducesResponseType<ICollection<SelectListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMemberSelectListAsync(
        CancellationToken cancellationToken)
    {
        var selectList = await _memberService.GetMemberSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{memberId:guid}")]
    [Authorize(Policy = Permissions.ViewMembers)]
    [EndpointSummary("Get member by ID")]
    [ProducesResponseType<MemberDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMemberByIdAsync(
        Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken);
        return Ok(member);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddMembers)]
    [EndpointSummary("Create member")]
    [ProducesResponseType<MemberDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddMemberAsync(
        [FromBody] MemberDto memberDto, CancellationToken cancellationToken)
    {
        var createdMember = await _memberService.AddMemberAsync(memberDto, cancellationToken);
        _metrics.Member.Created(createdMember.Id.ToString());
        return CreatedAtAction(nameof(AddMemberAsync), new { memberId = createdMember.Id }, createdMember);
    }

    [HttpDelete("{memberId:guid}")]
    [Authorize(Policy = Permissions.DeleteMembers)]
    [EndpointSummary("Delete member")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMemberAsync(
        Guid memberId, CancellationToken cancellationToken)
    {
        await _memberService.DeleteMemberAsync(memberId, cancellationToken);
        _metrics.Member.Deleted();
        return NoContent();
    }

    [HttpPut("{memberId:guid}")]
    [Authorize(Policy = Permissions.EditMembers)]
    [EndpointSummary("Update member")]
    [ProducesResponseType<MemberDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMemberAsync(
        Guid memberId,
        [FromBody] MemberDto memberDto,
        CancellationToken cancellationToken)
    {
        var updatedMember = await _memberService.UpdateMemberAsync(memberId, memberDto, cancellationToken);
        return Ok(updatedMember);
    }
}
