using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.Entities.Electricity;

namespace GSManager.Core.Mappers;

public static class ElectricityMeterMapper
{
    public static ElectricityMeterDto ToDto(ElectricityMeter meter)
    {
        return new ElectricityMeterDto
        {
            Id = meter.Id,
            Name = meter.Name,
            SerialNumber = meter.SerialNumber,
            Location = meter.Location,
            InstallationDate = meter.InstallationDate,
            LastMaintenanceDate = meter.LastMaintenanceDate,
            Notes = meter.Notes,
            PlotId = meter.PlotId,
            OwnerId = meter.OwnerId
        };
    }

    public static ElectricityMeter ToEntity(ElectricityMeterDto dto)
    {
        return new ElectricityMeter
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            SerialNumber = dto.SerialNumber,
            Location = dto.Location,
            InstallationDate = dto.InstallationDate,
            LastMaintenanceDate = dto.LastMaintenanceDate,
            Notes = dto.Notes,
            PlotId = dto.PlotId,
            OwnerId = dto.OwnerId
        };
    }
}
