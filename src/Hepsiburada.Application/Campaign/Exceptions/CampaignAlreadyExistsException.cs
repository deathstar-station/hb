using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Campaign.Exceptions
{
    public class CampaignAlreadyExistsException : HepsiburadaExceptionBase
    {
        public CampaignAlreadyExistsException(string name) : 
            base($"There is already exists campaign with following name:{name}") { }
    }
}
