using Ambev.DeveloperEvaluation.Application.Carts.CommonValidator;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        private readonly IProductRepository _productRepository;

        public CreateSaleCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            // Validação para UserId
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            // Validação para Products
            RuleFor(command => command.Products)
                .NotEmpty().WithMessage("Products list cannot be empty.")
                .Must(products => products.All(p => p != null)).WithMessage("Products list cannot contain null values.")
                .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All products must have a quantity greater than zero.");

            // Validação para cada produto individualmente
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
