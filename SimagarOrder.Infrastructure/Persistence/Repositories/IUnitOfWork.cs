using Microsoft.EntityFrameworkCore;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Infrastructure.Persistence.Repositories;
public interface IUnitOfWork : IDisposable, IScopedDependency
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync (CancellationToken cancellationToken);
    Task CommitAsync (CancellationToken cancellationToken);
    Task RollbackAsync (CancellationToken cancellationToken);

}
