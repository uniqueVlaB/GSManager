using FluentValidation;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.FluentValidation;

public class PriviledgeDtoValidator : AbstractValidator<PriviledgeDto>
{
    public PriviledgeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Priviledge name is required.")
            .MaximumLength(100)
            .WithMessage("Priviledge name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
