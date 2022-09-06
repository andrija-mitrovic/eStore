using FluentValidation;

namespace Application.Products.Commands.UpdateProduct
{
    internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PictureUrl).NotEmpty();
            RuleFor(x => x.Brand).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.QuantityInStock).GreaterThan(0).When(x => x != null);
        }
    }
}
