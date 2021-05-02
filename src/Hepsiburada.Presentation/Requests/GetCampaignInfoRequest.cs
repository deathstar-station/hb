using System.Threading.Tasks;
using Hepsiburada.Application.Campaign.Queries.GetCampaignInfo;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public class GetCampaignInfoRequest : Request
    {
        public GetCampaignInfoRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 2)
                return false;

            if (_parameters[0] != "get_campaign_info")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var query = new GetCampaignInfoQuery
            {
                CampaignName = _parameters[1]
            };

            var result = await _mediator.Send(query);

            return $"Campaign {result.Name} info;" +
                   $" Status {(result.IsActive ? "Active" : "Ended")}," +
                   $" Target Sales {result.TargetSales}," +
                   $"Total Sales {result.TotalSales}," +
                   $" Turnover {result.Turnover}," +
                   $" Average Item Price {result.AverageItemPrice} \r\n";
        }
    }
}