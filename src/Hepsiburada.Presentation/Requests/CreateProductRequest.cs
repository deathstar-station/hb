using System;
using System.Threading.Tasks;
using Hepsiburada.Application.Product.Commands.CreateProduct;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public class CreateProductRequest : Request
    {
        public CreateProductRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 4)
                return false;

            if (_parameters[0] != "create_product")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]))
                return false;

            if (string.IsNullOrEmpty(_parameters[2]) && !decimal.TryParse(_parameters[2], out _))
                return false;

            if (string.IsNullOrEmpty(_parameters[3]) && !int.TryParse(_parameters[3], out _))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var command = new CreateProductCommand
            {
                ProductCode = _parameters[1],
                Price = decimal.Parse(_parameters[2]),
                Stock = int.Parse(_parameters[3])
            };
            var response = await _mediator.Send(command);
            return $"Product created; code {response.Product.ProductCode} , price {response.Product.Price} , stock {response.Product.Stock} \r\n";
        }
    }
}