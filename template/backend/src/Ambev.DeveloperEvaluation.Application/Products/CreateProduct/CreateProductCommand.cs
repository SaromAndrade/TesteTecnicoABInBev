using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    /// <summary>
    /// Command to create a new product.
    /// </summary>
    public class CreateProductCommand : IRequest<CreateProductResult>
    {

        /// <summary>
        /// Title of the product.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Category of the product.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Image URL of the product.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Rating of the product.
        /// </summary>
        public RatingDto Rating { get; set; }
    }
    /// <summary>
    /// Represents the rating of a product.
    /// </summary>
    public class RatingDto
    {
        /// <summary>
        /// Rate of the product.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Count of ratings.
        /// </summary>
        public int Count { get; set; }
    }
}
