namespace exam_system.Persistence.DataAccess;

public interface IUnitOfWork : IDisposable
{
    // New transaction flow
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken);
    Task AddSavePointAsync(string name, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    // Legacy transaction flow
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

}
