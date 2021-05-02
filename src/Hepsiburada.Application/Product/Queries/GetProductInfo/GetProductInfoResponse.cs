using Hepsiburada.Domain.Product;

namespace Hepsiburada.Application.Product.Queries.GetProductInfo
{
    public class GetProductInfoResponse
    {
        public GetProductInfoResponse(ProductEntity productResponse, decimal sellingPrice)
        {
            ProductCode = productResponse.ProductCode;
            Price = sellingPrice;
            Stock = productResponse.Stock;
        }

        public string ProductCode { get; }
        public decimal Price { get; }
        public int Stock { get; }
    }
}
