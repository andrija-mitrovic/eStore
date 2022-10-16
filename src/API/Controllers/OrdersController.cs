using Application.Common.DTOs;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries.GetOrderById;
using Application.Orders.Queries.GetOrders;
using Domain.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders(CancellationToken cancellationToken)
        {
            var query = new GetOrdersQuery() 
            { 
                BuyerId = User?.Identity?.Name 
            };

            return await Mediator.Send(query, cancellationToken);
        }
        
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<Order?>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var query = new GetOrderByIdQuery() 
            { 
                Id = id, 
                BuyerId = User?.Identity?.Name 
            };

            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrder order, CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand()
            {
                Username = User?.Identity?.Name,
                BuyerId = User?.Identity?.Name,
                SaveAddress = order.SaveAddress,
                ShippingAddress = order.ShippingAddress
            };

            var orderId = await Mediator.Send(command, cancellationToken);

            return CreatedAtAction("GetOrder", new { id = orderId }, orderId);
        }
    }
}
