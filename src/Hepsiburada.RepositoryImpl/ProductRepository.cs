using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.RepositoryImpl
{
    public class ProductRepository : IRepository<ProductEntity>
    {
        private static readonly List<ProductEntity> _products = new();
        private static int _currentId = 0;

        public Task<ProductEntity> AddAsync(ProductEntity entity)
        {
            if (entity.Id > 0) throw new InvalidEntityException("Entity id must be zero if you add");

            ((IEntityBase)entity).SetId(++_currentId);
            _products.Add(entity);

            return Task.FromResult(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<ProductEntity, bool>> expression)
        {
            return Task.FromResult(_products.Any(expression.Compile()));
        }

        public Task<IEnumerable<ProductEntity>> FindAsync(Expression<Func<ProductEntity, bool>> expression)
        {
            return Task.FromResult(_products.Where(expression.Compile()).Select(x => x));
        }

        public Task<ProductEntity> FindOneAsync(Expression<Func<ProductEntity, bool>> expression)
        {
            return Task.FromResult(_products.FirstOrDefault(expression.Compile()));
        }

        public Task UpdateAsync(ProductEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
