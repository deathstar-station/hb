using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Campaign.Exceptions
{
    public class InvalidPriceManipulationLimitException : HepsiburadaExceptionBase
    {
        public InvalidPriceManipulationLimitException(int priceManipulationLimit) : 
            base( $"Invalid price manipulation limit value: {priceManipulationLimit}, Value must be bigger than zero and lower than 101")
        {
            this.PriceManipulationLimit = priceManipulationLimit;
        }

        public int PriceManipulationLimit { get; private set; }
    }
}
