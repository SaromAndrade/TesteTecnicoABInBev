using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    public class CreateProductResponse
    {
        /// <summary>
        /// The ID of the created product.
        /// </summary>
        public int Id { get; set; }

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
        public RatingResponse Rating { get; set; }
    }
}
