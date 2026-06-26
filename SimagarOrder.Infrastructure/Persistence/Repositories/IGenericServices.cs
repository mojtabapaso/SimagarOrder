namespace SimagarOrder.Infrastructure.Persistence.Repositories;

public interface IGenericServices<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(long Id);
    TEntity FindById(long id);
    Task<TEntity> FindByIdAsync(long id);
    List<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
}
