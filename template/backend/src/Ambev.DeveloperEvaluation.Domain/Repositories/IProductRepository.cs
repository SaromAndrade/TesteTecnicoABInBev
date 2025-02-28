using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Product?> CreateAsync(Product product, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves a paginated list of products from the database.
        /// </summary>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="size">The number of items per page (default is 10).</param>
        /// <param name="order">Sorting order (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A paginated list of products.</returns>
        Task<(List<Product> Products, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(string id, Product product, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves products by category.
        /// </summary>
        /// <param name="category">The category to filter products by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of products in the specified category.</returns>
        Task<List<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
    }
}
