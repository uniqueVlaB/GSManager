using FluentValidation;
using GSManager.Core.Models.DTOs.Entities;

namespace GSManager.Core.FluentValidation;

public class PlotDtoValidator : AbstractValidator<PlotDto>
{
    public PlotDtoValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Plot number is required.")
            .MaximumLength(50)
            .WithMessage("Plot number must not exceed 50 characters.");

        RuleFor(x => x.CadastreNumber)
            .MaximumLength(100)
            .When(x => x.CadastreNumber is not null)
            .WithMessage("Cadastre number must not exceed 100 characters.");

        RuleFor(x => x.Square)
            .GreaterThan(0)
            .When(x => x.Square.HasValue)
            .WithMessage("Square must be greater than 0.");
    }
}
