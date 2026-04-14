using GSManager.Core.Abstractions.Repository;
using GSManager.Infrastructure.SQL.Database;
using GSManager.Infrastructure.SQL.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSManager.Infrastructure.SQL;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Plots = new PlotRepository(_dbContext);
        Roles = new RoleRepository(_dbContext);
        Priviledges = new PriviledgeRepository(_dbContext);
        Members = new MemberRepository(_dbContext);
        IdentityRoles = new IdentityRoleRepository(_dbContext);
        IdentityRolesClaims = new IdentityRoleClaimRepository(_dbContext);
        IdentityUserRoles = new IdentityUserRoleRepository(_dbContext);
        IdentityUserClaims = new IdentityUserClaimRepository(_dbContext);
        RefreshTokens = new RefreshTokenRepository(_dbContext);
        ElectricityMeters = new ElectricityMeterRepository(_dbContext);
        ElectricityReadings = new ElectricityReadingRepository(_dbContext);
    }

    public IMemberRepository Members { get; private set; }
    public IPlotRepository Plots { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public IIdentityRoleRepository IdentityRoles { get; private set; }
    public IIdentityRoleClaimsRepository IdentityRolesClaims { get; private set; }
    public IIdentityUserRoleRepository IdentityUserRoles { get; private set; }
    public IIdentityUserClaimRepository IdentityUserClaims { get; private set; }
    public IRefreshTokenRepository RefreshTokens { get; private set; }
    public IPriviledgeRepository Priviledges { get; private set; }
    public IElectricityMeterRepository ElectricityMeters { get; private set; }
    public IElectricityReadingRepository ElectricityReadings { get; private set; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return;
        }

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return;
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
