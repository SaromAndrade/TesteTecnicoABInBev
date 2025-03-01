using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CommonValidator
{
    public class CartProductDtoValidator : AbstractValidator<CartProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CartProductDtoValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than zero.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
