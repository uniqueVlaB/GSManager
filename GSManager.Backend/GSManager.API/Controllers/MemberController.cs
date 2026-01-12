using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs;

namespace GSManager.API.Controllers;

[ApiController]
[Route("members")]
public class MemberController(IMemberService memberService) : ControllerBase
{
    private readonly IMemberService _memberService = memberService;

    [HttpGet]
    public async Task<IActionResult> GetFilteredMembersAsync([FromQuery] MemberFilterDto filterDto, CancellationToken cancellationToken)
    {
        ICollection<MemberDto> dtos;
        if (filterDto is null)
        {
            dtos = await _memberService.GetAllMembersAsync(cancellationToken);
        }
        else
        {
            dtos = await _memberService.GetFilteredMembersAsync(filterDto, cancellationToken);
        }

        return Ok(dtos);
    }

    [HttpGet("select-list")]
    public async Task<IActionResult> GetMemberSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _memberService.GetMemberSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{memberId:guid}")]
    public async Task<IActionResult> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken);
        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> AddMemberAsync([FromBody] MemberDto memberDto, CancellationToken cancellationToken)
    {
        var createdMember = await _memberService.AddMemberAsync(memberDto, cancellationToken);
        return CreatedAtAction(nameof(AddMemberAsync), new { memberId = createdMember.Id }, createdMember);
    }

    [HttpDelete("{memberId:guid}")]
    public async Task<IActionResult> DeleteMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        await _memberService.DeleteMemberAsync(memberId, cancellationToken);
        return NoContent();
    }
}
