using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Time.Exceptions
{
    public class InvalidHourException : HepsiburadaExceptionBase
    {
        public InvalidHourException() : base("Hour must be greater than zero")
        {
        }
    }
}
