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
    public async Task<IActionResult> GetAllMembersAsync(CancellationToken cancellationToken)
    {
        var dtos = await _memberService.GetAllMembersAsync(cancellationToken).ConfigureAwait(false);
        return Ok(dtos);
    }

    [HttpGet("{memberId:guid}")]
    public async Task<IActionResult> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken).ConfigureAwait(false);
        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> AddMemberAsync([FromBody] MemberDto memberDto, CancellationToken cancellationToken)
    {
        var createdMember = await _memberService.AddMemberAsync(memberDto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(AddMemberAsync), new { memberId = createdMember.Id }, createdMember);
    }
}
