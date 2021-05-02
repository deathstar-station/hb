using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Domain.Order;

namespace Hepsiburada.Application.Order.Commands.CreateOrder
{
    public class CreateOrderResponse 
    {
        public CreateOrderResponse(OrderEntity order)
        {
            Order = order;
        }

        public  OrderEntity Order { get; }
    }
}
