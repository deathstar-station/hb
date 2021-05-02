using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Product.Exceptions
{
    public class InsufficientStockException : HepsiburadaExceptionBase
    {
        public InsufficientStockException() : base("Insufficient Stock.") { }
    }
}