using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Campaign.Exceptions
{
    public class AlreadyActiveCampaignForProductException : HepsiburadaExceptionBase
    {
        public AlreadyActiveCampaignForProductException(string productCode) : 
            base($"There is already active campaign for product:{productCode}") { }
    }
}
