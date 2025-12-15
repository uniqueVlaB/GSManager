using GSManager.Core.Abstractions.Repository;
using GSManager.Infrastructure.SQL.Database;
using GSManager.Infrastructure.SQL.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSManager.Infrastructure.SQL;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    private readonly object _syncRoot = new();

    private IDbContextTransaction? _transaction;

    private bool _disposed;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Plots = new PlotRepository(_dbContext);
        Roles = new RoleRepository(_dbContext);
        Priviledges = new PriviledgeRepository(_dbContext);
        Members = new MemberRepository(_dbContext);
    }

    public IMemberRepository Members { get; private set; }

    public IPlotRepository Plots { get; private set; }

    public IRoleRepository Roles { get; private set; }

    public IPriviledgeRepository Priviledges { get; private set; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        lock (_syncRoot)
        {
            if (_transaction is not null)
            {
                return;
            }
        }

        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        lock (_syncRoot)
        {
            _transaction = transaction;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        IDbContextTransaction? transaction;
        lock (_syncRoot)
        {
            transaction = _transaction;
            if (transaction is null)
            {
                return;
            }
        }

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            lock (_syncRoot)
            {
                transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        IDbContextTransaction? transaction;
        lock (_syncRoot)
        {
            transaction = _transaction;
            if (transaction is null)
            {
                return;
            }
        }

        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            lock (_syncRoot)
            {
                transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        lock (_syncRoot)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
