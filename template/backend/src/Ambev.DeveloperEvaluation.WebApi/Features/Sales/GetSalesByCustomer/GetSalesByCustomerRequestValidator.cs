using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerRequestValidator : AbstractValidator<GetSalesByCustomerRequest>
    {
        public GetSalesByCustomerRequestValidator()
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");

            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(query => query.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must be less than or equal to 100.");

            RuleFor(query => query.Order)
                .Must(BeAValidOrderExpression).When(query => !string.IsNullOrEmpty(query.Order))
                .WithMessage("Order must be in the format 'field [asc|desc]' (e.g., 'createdAt desc').");
        }
        /// <summary>
        /// Valida se a expressão de ordenação é válida.
        /// </summary>
        private bool BeAValidOrderExpression(string order)
        {
            // Divide a expressão em partes (ex: "createdAt desc, totalPrice asc")
            var orderParams = order.Split(',');

            foreach (var param in orderParams)
            {
                var orderBy = param.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (orderBy.Length < 1 || orderBy.Length > 2)
                    return false;

                var property = orderBy[0].ToLower();
                var direction = orderBy.Length > 1 ? orderBy[1].ToLower() : "asc";

                if (!IsValidProperty(property) || !IsValidDirection(direction))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Verifica se a propriedade de ordenação é válida.
        /// </summary>
        private bool IsValidProperty(string property)
        {
            var validProperties = new[]
            {
                "id", "salenumber", "date", "totalamount", "finalamount",
                "discountamount", "branchid", "iscancelled"
            };
            return validProperties.Contains(property.ToLower());
        }

        /// <summary>
        /// Verifica se a direção de ordenação é válida.
        /// </summary>
        private bool IsValidDirection(string direction)
        {
            return direction == "asc" || direction == "desc";
        }
        private bool BeAValidGuid(string userId)
        {
            return Guid.TryParse(userId, out _);
        }
    }
}
