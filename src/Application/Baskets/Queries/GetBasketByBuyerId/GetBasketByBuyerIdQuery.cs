using Application.Common.DTOs;
using MediatR;

namespace Application.Baskets.Queries.GetBasketByBuyerId
{
    public class GetBasketByBuyerIdQuery : IRequest<BasketDto>
    {
        public string? BuyerId { get; set; }
    }
}
