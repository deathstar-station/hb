using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.Domain.Order
{
    public class OrderEntity : EntityBase
    {
        protected OrderEntity() { }
        public OrderEntity(string productCode, int quantity, decimal price)
        {
            if (string.IsNullOrEmpty(productCode)) throw new ArgumentRequiredException(nameof(productCode));
            if (quantity <= 0) throw new ValueMustBeBiggerThanZeroException(nameof(quantity), quantity);
            if (price <= 0) throw new ValueMustBeBiggerThanZeroException(nameof(price), price);

            ProductCode = productCode;
            Quantity = quantity;
            Price = price;
        }

        public OrderEntity(string productCode, int quantity, decimal price, int appliedCampaignId) : this(productCode, quantity, price)
        {
            if (appliedCampaignId <= 0) throw new EntityDoesNotExistsException();

            AppliedCampaignId = appliedCampaignId;
        }

        public string ProductCode { get; private set; }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public int? AppliedCampaignId { get; private set; }
    }
}
