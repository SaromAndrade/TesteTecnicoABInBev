using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
    {
        public DeleteSaleCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("The sale Id must be a positive number.");
        }
    }
}
