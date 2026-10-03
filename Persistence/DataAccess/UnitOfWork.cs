using exam_system.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace exam_system.Persistence.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private int _depth = 0;
    private string? _currentSavePoint;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        var isOutermost = _depth == 0;

        if (isOutermost)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _currentSavePoint = null;
        }

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
            if (isOutermost)
            {
                if (_currentSavePoint is not null)
                {
                    await _transaction!.RollbackToSavepointAsync(_currentSavePoint, CancellationToken.None);
                    _context.ChangeTracker.Clear();
                    await _transaction.CommitAsync(CancellationToken.None);
                }

                await _transaction!.RollbackAsync(CancellationToken.None);
            }

            throw;
        }
        finally
        {
            _depth--;

            if (isOutermost)
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
                _currentSavePoint = null;
            }
        }
    }

    public async Task AddSavePointAsync(string savePointName, CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to create a savepoint.");

        await _context.SaveChangesAsync(cancellationToken);
        await _transaction.CreateSavepointAsync(savePointName, cancellationToken);
        _currentSavePoint = savePointName;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    
    #region Old Transaction methods & Legacy transaction flow
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

    
    #endregion

}
