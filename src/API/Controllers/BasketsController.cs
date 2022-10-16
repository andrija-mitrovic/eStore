using Application.Baskets.Commands.AddBasketItem;
using Application.Baskets.Commands.DeleteBasketItem;
using Application.Baskets.Queries.GetBasketByBuyerId;
using Application.Common.Constants;
using Application.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketsController : ApiControllerBase
    {
        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket(CancellationToken cancellationToken)
        {
            var buyerId = GetBuyerId();

            return await Mediator.Send(new GetBasketByBuyerIdQuery() { BuyerId = buyerId }, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddItemToBasket(int productId, int quantity, CancellationToken cancellationToken)
        {
            var command = new AddBasketItemCommand()
            {
                ProductId = productId,
                Quantity = quantity,
                BuyerId = GetBuyerId()
            };

            return CreatedAtRoute("GetBasket", await Mediator.Send(command, cancellationToken));
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity, CancellationToken cancellationToken)
        {
            var command = new DeleteBasketItemCommand()
            {
                ProductId = productId,
                Quantity = quantity,
                BuyerId = GetBuyerId()
            };

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        private string? GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies[CookieConstants.KEY];
        }
    }
}

