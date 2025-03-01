using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new sale in the database.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Sale?> CreateAsync(Sale sale, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale with the specified ID, or null if not found.</returns>
        Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a paginated list of sales from the database.
        /// </summary>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="size">The number of items per page (default is 10).</param>
        /// <param name="order">Sorting order (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A paginated list of sales.</returns>
        Task<(List<Sale> Sales, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing sale in the database.
        /// </summary>
        /// <param name="id">The ID of the sale to update.</param>
        /// <param name="sale">The updated sale data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Sale> UpdateAsync(int id, Sale sale, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<bool> CancelSaleAsync(int id, CancellationToken cancellationToken);

        Task<(List<Sale> Sales, int TotalItems)> GetByCustomerAsync(string customerId, int page, int size, string order, CancellationToken cancellationToken);
    }
}
