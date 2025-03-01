using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            // Validação para UserId
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");
            // Validação para UserId
            RuleFor(command => command.BranchId)
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");

            // Validação para Products
            RuleFor(command => command.Products)
                .NotEmpty().WithMessage("Products list cannot be empty.")
                .Must(products => products.All(p => p != null)).WithMessage("Products list cannot contain null values.");
            // Validação para cada SaleProductDto dentro de Products
            RuleForEach(command => command.Products).SetValidator(new SaleProductDtoValidator());
        }
        private bool BeAValidGuid(string userId)
        {
            return Guid.TryParse(userId, out _);
        }
    }
}
