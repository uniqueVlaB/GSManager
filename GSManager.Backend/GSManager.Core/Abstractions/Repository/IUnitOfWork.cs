using GSManager.Core.Abstractions.Repository.Accounting;
using GSManager.Core.Abstractions.Repository.Auth;
using GSManager.Core.Abstractions.Repository.Electricity;
using GSManager.Core.Abstractions.Repository.Society;

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
    IPaymentRepository Payments { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}
