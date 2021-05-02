using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Campaign.Exceptions
{
    public class InvalidDurationException : HepsiburadaExceptionBase
    {
        public InvalidDurationException(int durationValue) : 
            base($"Invalid duration value: {durationValue}, Value must be bigger than zero")
        {
            this.DurationValue = durationValue;
        }

        public int DurationValue { get; private set; }
    }
}
