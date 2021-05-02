using System.Globalization;
using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Shared.Exceptions
{
    public class ValueMustBeBiggerThanZeroException : HepsiburadaExceptionBase
    {
        public ValueMustBeBiggerThanZeroException(string nameOfArgument, string value) : 
            base($"Invalid {nameOfArgument} value: {value}, Value must be bigger than zero") { }

        public ValueMustBeBiggerThanZeroException(string nameOfArgument, decimal value) : this(nameOfArgument, value.ToString(CultureInfo.InvariantCulture)) { }

        public ValueMustBeBiggerThanZeroException(string nameOfArgument, int value) : this(nameOfArgument, value.ToString()) { }
    }
}
