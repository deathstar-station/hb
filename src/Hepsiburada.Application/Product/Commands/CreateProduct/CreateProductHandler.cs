using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Product.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public CreateProductHandler(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var isProductExists = await _productRepository.AnyAsync(x => x.ProductCode == request.ProductCode);
            if (isProductExists)
                throw new ProductAlreadyExistsException(request.ProductCode);

            var product = await _productRepository.AddAsync(new ProductEntity(request.ProductCode, request.Price, request.Stock));
            
            return new CreateProductResponse(product);
        }
    }
}
