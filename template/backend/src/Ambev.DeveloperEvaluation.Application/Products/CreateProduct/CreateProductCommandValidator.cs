using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(2, 100).WithMessage("Title must be between 2 and 100 characters.");

            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

            RuleFor(product => product.Category)
                .NotEmpty().WithMessage("Category is required.")
                .Length(2, 50).WithMessage("Category must be between 2 and 50 characters.");

            RuleFor(product => product.Image)
                .Must(BeAValidUrl).When(product => !string.IsNullOrEmpty(product.Image))
                .WithMessage("Image must be a valid URL.");

            RuleFor(product => product.Rating.Rate)
                .InclusiveBetween(0, 5).WithMessage("Rating rate must be between 0 and 5.");

            RuleFor(product => product.Rating.Count)
                .GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to 0.");
        }
        /// <summary>
        /// Validates if the provided string is a valid URL.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        /// <returns>True if the URL is valid, otherwise false.</returns>
        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
