using Hepsiburada.Domain.Product.Exceptions;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.Domain.Product
{
    public class ProductEntity : EntityBase
    {
        protected ProductEntity() { }

        public ProductEntity(string productCode, decimal price, int stock)
        {
            if (string.IsNullOrEmpty(productCode)) throw new ArgumentRequiredException(nameof(productCode));
            if (price <= 0.0M) throw new ValueMustBeBiggerThanZeroException(nameof(price), price);
            if (stock < 0) throw new ValueCannotBeLowerThanZeroException(nameof(stock), stock);

            ProductCode = productCode;
            Price = price;
            Stock = stock;
        }

        public virtual string ProductCode { get; private set; }

        public virtual int Stock { get; private set; }

        public virtual decimal Price { get; private set; }

        public virtual void DecreaseStock(int quantity)
        {
            if (Stock < quantity)
            {
                throw new InsufficientStockException();
            }

            Stock -= quantity;
        }
    }
}
