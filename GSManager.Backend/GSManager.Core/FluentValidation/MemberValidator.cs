using FluentValidation;
using GSManager.Core.Models.DTOs.Entities;

namespace GSManager.Core.FluentValidation;

internal sealed class MemberValidator : AbstractValidator<MemberDto>
{
    public MemberValidator()
    {
        RuleFor(member => member.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(member => member.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}
