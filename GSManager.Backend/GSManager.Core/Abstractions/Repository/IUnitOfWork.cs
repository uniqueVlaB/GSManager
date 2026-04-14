namespace GSManager.Core.Abstractions.Repository;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository Members { get; }
    IPlotRepository Plots { get; }
    IRoleRepository Roles { get; }
    IIdentityRoleRepository IdentityRoles { get; }
    IIdentityRoleClaimsRepository IdentityRolesClaims { get; }
    IIdentityUserRoleRepository IdentityUserRoles { get; }
    IIdentityUserClaimRepository IdentityUserClaims { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IPriviledgeRepository Priviledges { get; }
    IElectricityMeterRepository ElectricityMeters { get; }
    IElectricityReadingRepository ElectricityReadings { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}
