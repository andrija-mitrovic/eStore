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
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var query = new GetOrdersQuery() 
            { 
                BuyerId = User?.Identity?.Name 
            };

            return await Mediator.Send(query);
        }
        
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<Order?>> GetOrder(int id)
        {
            var query = new GetOrderByIdQuery() 
            { 
                Id = id, 
                BuyerId = User?.Identity?.Name 
            };

            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderCommand command)
        {
            return CreatedAtAction("GetOrder", await Mediator.Send(command));
        }
    }
}
