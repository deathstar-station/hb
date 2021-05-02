using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.RepositoryImpl
{
    public class OrderRepository : IRepository<OrderEntity>
    {
        private static readonly List<OrderEntity> _orders = new();
        private static int _currentId = 0;

        public Task<OrderEntity> AddAsync(OrderEntity entity)
        {
            if (entity.Id > 0) throw new InvalidEntityException("Entity id must be zero if you add");

            ((IEntityBase)entity).SetId(++_currentId);
            _orders.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<OrderEntity, bool>> expression)
        {
            return Task.FromResult(_orders.Any(expression.Compile()));
        }

        public Task<IEnumerable<OrderEntity>> FindAsync(Expression<Func<OrderEntity, bool>> expression)
        {
            return Task.FromResult(_orders.Where(expression.Compile()).Select(x => x));
        }

        public Task<OrderEntity> FindOneAsync(Expression<Func<OrderEntity, bool>> expression)
        {
            return Task.FromResult(_orders.FirstOrDefault(expression.Compile()));
        }

        public Task UpdateAsync(OrderEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
