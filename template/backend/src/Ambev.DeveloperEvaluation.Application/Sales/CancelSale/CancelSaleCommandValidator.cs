using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleCommandValidator()
        {
            RuleFor(command => command.SaleNumber)
                .GreaterThan(0).WithMessage("SaleNumber must be greater than zero.");
        }
    }
}
