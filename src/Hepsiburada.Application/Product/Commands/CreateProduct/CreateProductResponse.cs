using Hepsiburada.Domain.Product;

namespace Hepsiburada.Application.Product.Commands.CreateProduct
{
    public class CreateProductResponse
    {
        public CreateProductResponse(ProductEntity product)
        {
            Product = product;
        }

        public ProductEntity Product { get; set; }
    }
}
