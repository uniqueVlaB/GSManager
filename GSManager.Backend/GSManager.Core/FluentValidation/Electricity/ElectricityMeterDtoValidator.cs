using FluentValidation;
using GSManager.Core.Models.DTOs.Entities.Electricity;

namespace GSManager.Core.FluentValidation.Electricity;

internal sealed class ElectricityMeterDtoValidator : AbstractValidator<ElectricityMeterDto>
{
    public ElectricityMeterDtoValidator()
    {
        RuleFor(meter => meter.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(meter => meter.SerialNumber)
            .NotEmpty().WithMessage("Serial number is required.")
            .MaximumLength(50).WithMessage("Serial number cannot exceed 50 characters.");

        RuleFor(meter => meter.Location)
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

        RuleFor(meter => meter.InstallationDate)
            .NotEmpty().WithMessage("Installation date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Installation date cannot be in the future.");

        When(meter => meter.LastMaintenanceDate.HasValue, () =>
            RuleFor(meter => meter.LastMaintenanceDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Last maintenance date cannot be in the future.")
                .GreaterThanOrEqualTo(meter => meter.InstallationDate).WithMessage("Last maintenance date must be after the installation date.")
        );

        RuleFor(meter => meter.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

        RuleFor(meter => meter.PlotId)
            .NotEmpty().WithMessage("Plot ID is required.");
    }
}

