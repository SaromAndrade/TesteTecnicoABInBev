using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeQueryValidator : AbstractValidator<GetSalesByDateRangeQuery>
    {
        public GetSalesByDateRangeQueryValidator()
        {
            RuleFor(query => query.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(query => query.EndDate)
            .WithMessage("Start date must be less than or equal to end date.");

            RuleFor(query => query.EndDate)
                .NotEmpty().WithMessage("End date is required.");

            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(query => query.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must be less than or equal to 100.");
        }
    }
}
