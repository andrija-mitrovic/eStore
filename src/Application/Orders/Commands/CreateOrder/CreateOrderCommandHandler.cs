using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.OrderAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Commands.CreateOrder
{
    internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private const int DELIVERY_FEE_THRESHOLD = 10000;
        private const int DEFAULT_DELIVERY_FEE = 500;
        private const int NO_DELIVERY_FEE = 0;

        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IBasketRepository basketRepository, 
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!);

            if (basket == null)
            {
                _logger.LogError(nameof(Basket) + " with Buyer Id: {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException(nameof(Product), request.BuyerId!);
            }

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _productRepository.GetByIdAsync(item.ProductId);
                
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem!.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > DELIVERY_FEE_THRESHOLD ? NO_DELIVERY_FEE : DEFAULT_DELIVERY_FEE;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = request.BuyerId,
                ShippingAddress = request.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
            };

            await _orderRepository.AddAsync(order);
            await _basketRepository.AddAsync(basket);

            if (request.SaveAddress)
            {
                await _userRepository.UpdateUserAddress(request.Username!, request.ShippingAddress!);
            }

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem creating order.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return order.Id;
        }
    }
}
