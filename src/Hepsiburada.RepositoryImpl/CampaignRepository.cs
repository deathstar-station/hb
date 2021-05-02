using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.RepositoryImpl
{
    public class CampaignRepository : IRepository<CampaignEntity>
    {
        private static readonly List<CampaignEntity> _campaigns = new();
        private static int _currentId = 0;

        public Task<CampaignEntity> AddAsync(CampaignEntity entity)
        {
            if (entity == null || entity.Id > 0) throw new InvalidEntityException();

            ((IEntityBase)entity).SetId(++_currentId);
            _campaigns.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<CampaignEntity, bool>> expression)
        {
            return Task.FromResult(_campaigns.Any(expression.Compile()));
        }

        public Task<IEnumerable<CampaignEntity>> FindAsync(Expression<Func<CampaignEntity, bool>> expression)
        {
            return Task.FromResult(_campaigns.Where(expression.Compile()).Select(x => x));
        }

        public Task<CampaignEntity> FindOneAsync(Expression<Func<CampaignEntity, bool>> expression)
        {
            return Task.FromResult(_campaigns.FirstOrDefault(expression.Compile()));
        }

        public Task UpdateAsync(CampaignEntity entity)
        {
            if (entity == null || entity.Id <= 0) throw new InvalidEntityException();

            return Task.CompletedTask;
        }
    }
}
