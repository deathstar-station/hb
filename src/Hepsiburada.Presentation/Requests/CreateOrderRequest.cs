using System;
using Hepsiburada.Application.Order.Commands.CreateOrder;
using MediatR;
using System.Threading.Tasks;

namespace Hepsiburada.Presentation.Requests
{
    public class CreateOrderRequest : Request
    {
        public CreateOrderRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 3)
                return false;

            if (_parameters[0] != "create_order")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]))
                return false;

            if (string.IsNullOrEmpty(_parameters[2]) && !decimal.TryParse(_parameters[2], out _))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var command = new CreateOrderCommand
            {
                ProductCode = _parameters[1],
                Quantity = int.Parse(_parameters[2])
            };

            var response = await _mediator.Send(command);

            return $"Order created; product {response.Order.ProductCode}, quantity {response.Order.Quantity} \r\n";
        }
    }
}