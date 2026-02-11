namespace GSManager.Core.Abstractions.Repository;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository Members { get; }

    IPlotRepository Plots { get; }

    IRoleRepository Roles { get; }

    IIdentityRoleRepository IdentityRoles { get; }

    IIdentityRoleClaimsRepository IdentityRolesClaims { get; }

    IRefreshTokenRepository RefreshTokens { get; }

    IPriviledgeRepository Priviledges { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}
