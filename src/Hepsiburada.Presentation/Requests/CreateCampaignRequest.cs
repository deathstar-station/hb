using System.Threading.Tasks;
using Hepsiburada.Application.Campaign.Commands.CreateCampaign;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public class CreateCampaignRequest : Request
    {
        public CreateCampaignRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 6)
                return false;

            if (_parameters[0] != "create_campaign")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]))
                return false;

            if (string.IsNullOrEmpty(_parameters[2]))
                return false;

            if (string.IsNullOrEmpty(_parameters[3]) && !int.TryParse(_parameters[3], out _))
                return false;

            if (string.IsNullOrEmpty(_parameters[4]) && !int.TryParse(_parameters[4], out _))
                return false;

            if (string.IsNullOrEmpty(_parameters[5]) && !int.TryParse(_parameters[5], out _))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var command = new CreateCampaignCommand
            {
                CampaignName = _parameters[1],
                ProductCode = _parameters[2],
                Duration = int.Parse(_parameters[3]),
                PriceManipulationLimit = int.Parse(_parameters[4]),
                TargetSalesCount = int.Parse(_parameters[5])
            };
            var response = await _mediator.Send(command);
            return "Campaign created; " +
                   $"name {response.Campaign.Name}," +
                   $" product {response.Campaign.ProductCode}," +
                   $" duration {response.Campaign.Duration}," +
                   $"limit {response.Campaign.PriceManipulationLimit}," +
                   $" target sales count {response.Campaign.TargetSalesCount} \r\n";
        }
    }
}