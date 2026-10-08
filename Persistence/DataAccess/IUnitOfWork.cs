namespace exam_system.Persistence.DataAccess;

public interface IUnitOfWork : IDisposable
{
    Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
    Task AddSavePointAsync(string savePointName, CancellationToken cancellationToken = default);
    Task RollbackToSavePointAsync(string savePointName, CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
