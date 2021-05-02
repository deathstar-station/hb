using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Shared.Exceptions
{
    public class ValueCannotBeLowerThanZeroException : HepsiburadaExceptionBase
    {
        public ValueCannotBeLowerThanZeroException(string nameOfArgument, string value) :
            base($"Invalid {nameOfArgument} value: {value}, Value cannot be lower than zero") { }

        public ValueCannotBeLowerThanZeroException(string nameOfArgument, int value) : this(nameOfArgument, value.ToString()) { }
    }
}
