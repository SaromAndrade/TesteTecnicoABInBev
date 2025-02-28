using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductQueryValidator : AbstractValidator<GetAllProductQuery>
    {
        public GetAllProductQueryValidator()
        {
            // Validação para o parâmetro 'page'
            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            // Validação para o parâmetro 'size'
            RuleFor(query => query.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must be less than or equal to 100.");

            // Validação para o parâmetro 'order'
            RuleFor(query => query.Order)
                .Must(BeAValidOrderExpression).When(query => !string.IsNullOrEmpty(query.Order))
                .WithMessage("Order must be in the format 'field [asc|desc]' (e.g., 'price desc').");
        }
        /// <summary>
        /// Valida se a expressão de ordenação é válida.
        /// </summary>
        /// <param name="order">A expressão de ordenação.</param>
        /// <returns>True se a expressão for válida, caso contrário, False.</returns>
        private bool BeAValidOrderExpression(string order)
        {
            // Divide a expressão em partes (ex: "price desc, title asc")
            var orderParams = order.Split(',');

            foreach (var param in orderParams)
            {
                var orderBy = param.Trim().Split(' ');
                if (orderBy.Length < 1 || orderBy.Length > 2)
                    return false;

                var property = orderBy[0];
                var direction = orderBy.Length > 1 ? orderBy[1] : "asc";

                // Verifica se a propriedade é válida
                if (!IsValidProperty(property))
                    return false;

                // Verifica se a direção é válida
                if (!IsValidDirection(direction))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Verifica se a propriedade de ordenação é válida.
        /// </summary>
        /// <param name="property">A propriedade de ordenação.</param>
        /// <returns>True se a propriedade for válida, caso contrário, False.</returns>
        private bool IsValidProperty(string property)
        {
            // Lista de propriedades válidas para ordenação
            var validProperties = new[] { "title", "price", "category" }; // Adicione mais propriedades, se necessário
            return validProperties.Contains(property.ToLower());
        }

        /// <summary>
        /// Verifica se a direção de ordenação é válida.
        /// </summary>
        /// <param name="direction">A direção de ordenação.</param>
        /// <returns>True se a direção for válida, caso contrário, False.</returns>
        private bool IsValidDirection(string direction)
        {
            // Direções válidas: "asc" ou "desc"
            var validDirections = new[] { "asc", "desc" };
            return validDirections.Contains(direction.ToLower());
        }
    }
}
