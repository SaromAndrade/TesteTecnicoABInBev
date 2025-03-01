using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateSaleCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(command => command.Id)
                    .GreaterThan(0).WithMessage("SaleNumber must be greater than zero.");

            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(command => command.Products)
                .NotEmpty().WithMessage("Products list cannot be empty.")
                .Must(products => products.All(p => p != null)).WithMessage("Products list cannot contain null values.")
                .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All products must have a quantity greater than zero.");

            RuleForEach(command => command.Products).CustomAsync(async (product, context, cancellationToken) =>
            {
                var exists = await _productRepository.ExistsAsync(product.ProductId, cancellationToken);
                if (!exists)
                {
                    context.AddFailure($"Product with ID {product.ProductId} does not exist.");
                }
            });
        }
    }
}
