﻿using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesQueryValidator : AbstractValidator<GetAllSalesQuery>
    {
        public GetAllSalesQueryValidator()
        {
            // Validação para o parâmetro 'Page'
            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            // Validação para o parâmetro 'Size'
            RuleFor(query => query.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must be less than or equal to 100.");

            // Validação para o parâmetro 'Order'
            RuleFor(query => query.Order)
                .Must(BeAValidOrderExpression).When(query => !string.IsNullOrEmpty(query.Order))
                .WithMessage("Order must be in the format 'field [asc|desc]' (e.g., 'createdAt desc').");
        }
        /// <summary>
        /// Valida se a expressão de ordenação é válida.
        /// </summary>
        private bool BeAValidOrderExpression(string order)
        {
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
            // Lista de propriedades válidas para ordenação
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
            // Direções válidas: "asc" ou "desc"
            return direction == "asc" || direction == "desc";
        }
    }
}
