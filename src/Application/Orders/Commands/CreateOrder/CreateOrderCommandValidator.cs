using FluentValidation;

namespace Application.Orders.Commands.CreateOrder
{
    internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.BuyerId).NotEmpty();
            RuleFor(x => x.ShippingAddress).NotEmpty();
        }
    }
}
