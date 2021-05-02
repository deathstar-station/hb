using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hepsiburada.Domain.Shared.Abstractions
{
    public interface IRepository<TEntity>
        where TEntity : IEntityBase
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        
        Task UpdateAsync(TEntity entity);
    }
}
