using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct
{
    public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("The product Id must be a positive number.");
        }
    }
}
