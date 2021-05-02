using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Product.Exceptions
{
    public class ProductNotFoundException : HepsiburadaExceptionBase
    {
        public ProductNotFoundException() : base("Ürün bulunamadı.") { }
    }
}
