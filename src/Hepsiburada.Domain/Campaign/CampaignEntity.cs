using Hepsiburada.Domain.Campaign.Exceptions;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;
using System;

namespace Hepsiburada.Domain.Campaign
{
    public class CampaignEntity : EntityBase
    {
        protected CampaignEntity() { }

        public CampaignEntity(string name, string productCode, int duration, int priceManipulationLimit, int targetSalesCount, int createdTime)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentRequiredException(nameof(name));
            if (string.IsNullOrEmpty(productCode)) throw new ArgumentRequiredException(nameof(productCode));
            if (duration <= 0) throw new InvalidDurationException(duration);
            if (priceManipulationLimit <= 0 || priceManipulationLimit > 100) throw new InvalidPriceManipulationLimitException(priceManipulationLimit);
            if (targetSalesCount <= 0) throw new ValueMustBeBiggerThanZeroException(nameof(targetSalesCount), targetSalesCount);

            Name = name;
            ProductCode = productCode;
            Duration = duration;
            PriceManipulationLimit = priceManipulationLimit;
            TargetSalesCount = targetSalesCount;
            CreatedTime = createdTime;
        }

        public virtual string Name { get; private set; }
        public virtual string ProductCode { get; private set; }
        public virtual int CreatedTime { get; private set; }
        public virtual int Duration { get; private set; }
        public virtual int PriceManipulationLimit { get; private set; }
        public virtual int TargetSalesCount { get; private set; }
        public virtual int TotalSalesCount { get; private set; }
        public virtual decimal Turnover { get; private set; }

        public decimal AverageItemPrice => Turnover > 0 ? Math.Floor(Turnover / TotalSalesCount) : 0;

        public virtual bool IsActive(int currentTime) => TotalSalesCount < TargetSalesCount && currentTime < CreatedTime + Duration;
        
        public virtual void NotifyOrderCreation(int quantity, decimal price)
        {
            if (quantity <= 0) throw new ValueMustBeBiggerThanZeroException(nameof(quantity), quantity);
            if (price <= 0) throw new ValueMustBeBiggerThanZeroException(nameof(price), price);

            Turnover += quantity * price;
            TotalSalesCount += quantity;
        }
    }
}
