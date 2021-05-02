using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Shared.Exceptions
{
    public class InvalidEntityException : HepsiburadaExceptionBase
    {
        public InvalidEntityException(string message = "Invalid Entity") : base(message) { }
    }
}