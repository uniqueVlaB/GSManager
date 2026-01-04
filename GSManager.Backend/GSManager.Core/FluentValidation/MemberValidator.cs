using FluentValidation;
using GSManager.Core.Models.DTOs;

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

        When(member => member.PlotIds is not null, () => RuleFor(member => member.PlotIds)
                .NotEmpty().WithMessage("Plot IDs list cannot be empty when provided."));
    }
}
