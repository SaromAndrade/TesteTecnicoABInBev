using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(command => command.Products)
                .NotEmpty().WithMessage("Products list cannot be empty.")
                .Must(products => products.All(p => p != null)).WithMessage("Products list cannot contain null values.")
                .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All products must have a quantity greater than zero.");

            // Validação para cada SaleProductDto dentro de Products
            RuleForEach(command => command.Products).SetValidator(new SaleProductDtoValidator());
        }
    }
}
