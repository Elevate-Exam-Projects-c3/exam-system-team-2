using exam_system.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace exam_system.Persistence.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private int _depth = 0;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        var isOutermost = _depth == 0;
        if(isOutermost)
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        _depth++;

        try
        {
            await action();
            if (isOutermost)
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _transaction!.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            if(isOutermost)
            {
                await _transaction!.RollbackAsync(cancellationToken);
            }
            throw;
        }
        finally { 
            _depth--;
            if (isOutermost)
            {
                await _transaction!.DisposeAsync();
                _transaction = null;
            }
        }
    }
    // Transaction methods
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
