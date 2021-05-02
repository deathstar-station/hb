using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Product.Exceptions
{
    public class ProductAlreadyExistsException : HepsiburadaExceptionBase
    {
        public ProductAlreadyExistsException(string code) : base($"Product already exists with following code:{code}.") { }
    }
}