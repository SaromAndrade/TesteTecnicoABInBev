using Ambev.DeveloperEvaluation.Application.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class SaleProductDtoValidator : AbstractValidator<SaleProductDto>
    {
        public SaleProductDtoValidator()
        {
            // Validação para ProductId
            RuleFor(product => product.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            // Validação para Quantity
            RuleFor(product => product.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("A maximum of 20 items per product is allowed.");

            // Validação para UnitPrice
            RuleFor(product => product.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");
        }
    }
}
