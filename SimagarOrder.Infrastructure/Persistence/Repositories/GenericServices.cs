using Microsoft.EntityFrameworkCore;
using SimagarOrder.Infrastructure.Persistence.Configurations;

namespace SimagarOrder.Infrastructure.Persistence.Repositories;

public class GenericServices<TEntity> : IGenericServices<TEntity> where TEntity : class, new()
{
    private readonly DbContextBasket context;
    private DbSet<TEntity> _entity;
    public GenericServices(DbContextBasket context)
    {
        this.context = context;
        _entity = context.Set<TEntity>();
    }
    public void Add(TEntity entity)
        => _entity.Add(entity);

    public async Task AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
    }

    public TEntity FindById(long id)
        => _entity.Find(id);

    public async Task<TEntity> FindByIdAsync(long id)
        => await _entity.FindAsync(id);

    public List<TEntity> GetAll()
        => _entity.ToList();

    public async Task<List<TEntity>> GetAllAsync()
        => await _entity.ToListAsync();

    public void Remove(TEntity entity)
        => _entity.Remove(entity);

    public void Remove(long Id)
    {
        var tEntity = new TEntity();
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null)
        {
            idProperty.SetValue(tEntity, Id, null);
        }
    }
    public void Update(TEntity entity)
        => _entity.Update(entity);
}