namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    public class CreateProductRequest
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
        public Rating Rating { get; set; }
    }
    public class Rating
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
