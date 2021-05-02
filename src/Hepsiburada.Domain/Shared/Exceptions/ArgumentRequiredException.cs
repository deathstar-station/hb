
using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Shared.Exceptions
{
    public class ArgumentRequiredException : HepsiburadaExceptionBase
    {
        public ArgumentRequiredException(string nameOfArgument) : base($"{nameOfArgument} is required.") { }
    }
}
