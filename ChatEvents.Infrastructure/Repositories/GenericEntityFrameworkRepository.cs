using System.Linq.Expressions;
using ChatEvents.Infrastructure.Database;
using ChatEvents.Models.DbEntities;
using ChatEvents.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatEvents.Infrastructure.Repositories;

public abstract class GenericEntityFrameworkRepository<TEntity>(ApplicationDbContext context) : IGenericOrmRepository<TEntity>
    where TEntity : DbEntity
{
    protected readonly DbSet<TEntity> Entities = context.Set<TEntity>();

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        EntityEntry<TEntity> addedEntity = await Entities.AddAsync(entity, cancellationToken);
        return addedEntity.Entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await Entities.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        Entities.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        Entities.UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        Entities.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        Entities.RemoveRange(entities);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await Entities.CountAsync(cancellationToken);
    }

    public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Where(predicate);
    }

    public virtual async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await Entities.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Entities.FindAsync([id], cancellationToken);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return Entities.AsQueryable();
    }
}