using System.Threading.Tasks;
using Hepsiburada.Application.Product.Queries.GetProductInfo;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public class GetProductInfoRequest : Request
    {
        public GetProductInfoRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 2)
                return false;

            if (_parameters[0] != "get_product_info")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var query = new GetProductInfoQuery
            {
                ProductCode = _parameters[1]
            };
            var result = await _mediator.Send(query);

            return $"Product {result.ProductCode} info; price {result.Price}, stock {result.Stock} \r\n";
        }
    }
}