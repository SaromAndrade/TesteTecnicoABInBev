namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    public class RatingResponse
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
