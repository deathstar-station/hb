using Bogus;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Product;

namespace Hepsiburada.Tests
{
    public class FakeObjects
    {
        private static readonly Faker<CampaignEntity> _campaignFaker;
        private static readonly Faker<ProductEntity> _productFaker;
        private static readonly Faker<OrderEntity> _orderFaker;

        static FakeObjects()
        {
            _campaignFaker = new Faker<CampaignEntity>()
                .CustomInstantiator(f => new CampaignEntity(f.Random.AlphaNumeric(3), f.Random.AlphaNumeric(3),
                    f.Random.Int(1, 10), f.Random.Int(1, 20), f.Random.Int(10, 100), 0));

            _productFaker = new Faker<ProductEntity>()
                .CustomInstantiator(f =>
                    new ProductEntity(f.Random.AlphaNumeric(3), f.Random.Decimal(10, 100), f.Random.Int(1, 20)));

            _orderFaker = new Faker<OrderEntity>()
                .CustomInstantiator(f => new OrderEntity(f.Random.AlphaNumeric(3), f.Random.Int(1, 10), f.Random.Decimal(1, 10)));

            Instance = new Faker();
        }

        public static Faker Instance { get; }

        public static CampaignEntity GenerateCampaign()
        {
            return _campaignFaker.Generate();
        }

        public static ProductEntity GenerateProduct()
        {
            return _productFaker.Generate();
        }

        public static OrderEntity GenerateOrder()
        {
            return _orderFaker.Generate();
        }


    }
}
