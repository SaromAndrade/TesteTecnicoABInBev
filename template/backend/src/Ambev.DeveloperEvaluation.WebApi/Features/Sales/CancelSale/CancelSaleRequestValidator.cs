using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
    {
        public CancelSaleRequestValidator()
        {
            RuleFor(command => command.SaleNumber)
                .GreaterThan(0).WithMessage("SaleNumber must be greater than zero.");
        }
    }
}
