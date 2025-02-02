using System.Linq.Expressions;
using ChatEvents.Models.DbEntities;

namespace ChatEvents.Repositories;

public interface IGenericOrmRepository<TEntity> where TEntity : DbEntity
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    Task<int> CountAsync(CancellationToken cancellationToken);
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken);

    Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken);
    IQueryable<TEntity> GetAll();
}