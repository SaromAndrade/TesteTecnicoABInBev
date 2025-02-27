using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    /// <summary>
    /// Represents the response after creating a new product.
    /// </summary>
    public class CreateProductResult
    {
        /// <summary>
        /// The ID of the created product.
        /// </summary>
        public string Id { get; set; }

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
        public RatingResult Rating { get; set; }
    }
    /// <summary>
    /// Represents the rating of a product in the response.
    /// </summary>
    public class RatingResult
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
