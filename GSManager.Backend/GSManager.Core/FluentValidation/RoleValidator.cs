using FluentValidation;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.FluentValidation;

internal sealed class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        RuleFor(role => role.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");

        RuleFor(role => role.Description)
            .MaximumLength(500).WithMessage("Role description must not exceed 500 characters.");
    }
}
