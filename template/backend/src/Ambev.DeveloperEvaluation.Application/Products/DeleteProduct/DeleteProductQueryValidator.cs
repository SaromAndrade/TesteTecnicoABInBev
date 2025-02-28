using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductQueryValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("The product Id must be a positive number.");
        }
    }
}
