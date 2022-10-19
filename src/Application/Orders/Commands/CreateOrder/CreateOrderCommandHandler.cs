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
            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!, cancellationToken);

            if (basket == null)
            {
                _logger.LogError(nameof(Basket) + " with Buyer Id: {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException($"{nameof(Basket)} with Buyer Id {request.BuyerId!} was not found.");
            }

            var orderItems = await ReturnOrderItems(basket, cancellationToken);
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > DELIVERY_FEE_THRESHOLD ? NO_DELIVERY_FEE : DEFAULT_DELIVERY_FEE;

            var order = new Order
            {
                OrderItems = orderItems,
                BuyerId = request.BuyerId,
                ShippingAddress = request.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
            };

            await _orderRepository.AddAsync(order, cancellationToken);
            _basketRepository.Delete(basket);

            if (request.SaveAddress) await AssignUserAddress(request.Username!, request.ShippingAddress!, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem creating order.";
                _logger.LogError(message);
                throw new OrderException(message);
            }

            return order.Id;
        }

        private async Task<List<OrderItem>> ReturnOrderItems(Basket basket, CancellationToken cancellationToken)
        {
            var items = new List<OrderItem>();

            var productIds = basket.Items.Select(x => x.ProductId);
            var productItems = await _productRepository.GetAsync(x => productIds.Contains(x.Id), cancellationToken: cancellationToken);

            foreach (var item in basket.Items)
            {
                var productItem = productItems.FirstOrDefault(x => x.Id == item.ProductId);

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

            return items;
        }

        private async Task AssignUserAddress(string username, ShippingAddress shippingAddress, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(username, cancellationToken);

            if (user == null) throw new NotFoundException($"User with username {username} was not found.");

            user.Address = new UserAddress
            {
                FullName = shippingAddress?.FullName,
                Address1 = shippingAddress?.Address1,
                Address2 = shippingAddress?.Address2,
                City = shippingAddress?.City,
                State = shippingAddress?.State,
                Zip = shippingAddress?.Zip,
                Country = shippingAddress?.Country
            };
        }
    }
}
