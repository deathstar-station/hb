using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Application.Campaign.Exceptions
{
    public class CampaignNotFoundException : HepsiburadaExceptionBase
    {
        public CampaignNotFoundException() : base("Campaign not found") { }
    }
}