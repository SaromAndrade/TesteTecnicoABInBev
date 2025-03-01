using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly MongoDbContext _context;

        public SaleRepository(MongoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the database.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<Sale?> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            sale.Id = await GetNextSequenceValueAsync("saleid", cancellationToken);
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync();
            return sale;
        }

        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale with the specified ID, or null if not found.</returns>
        public async Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Retrieves a paginated list of sales from the database.
        /// </summary>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="size">The number of items per page (default is 10).</param>
        /// <param name="order">Sorting order (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A paginated list of sales.</returns>
        public async Task<(List<Sale> Sales, int TotalItems)> GetAllAsync(int page = 1, int size = 10, string order = "", CancellationToken cancellationToken = default)
        {
            var query = _context.Sales.AsQueryable();

            query = Order(order, query);

            int totalItems = await _context.Sales.CountAsync(cancellationToken);

            var sales = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);

            return (sales, totalItems);
        }

        /// <summary>
        /// Updates an existing sale in the database.
        /// </summary>
        /// <param name="id">The ID of the sale to update.</param>
        /// <param name="sale">The updated sale data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<Sale> UpdateAsync(int id, Sale sale, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentException("Sale ID must be greater than zero.", nameof(id));

            if (sale == null)
                throw new ArgumentNullException(nameof(sale), "Sale data cannot be null.");

            try
            {
                var existingSale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);

                if (existingSale == null)
                    throw new KeyNotFoundException($"Sale with ID {id} not found.");

                existingSale.CustomerId = sale.CustomerId;
                existingSale.IsCancelled = sale.IsCancelled;

                existingSale.Products.Clear();
                existingSale.Products = sale.Products;

                existingSale.TotalAmount = sale.TotalAmount;
                existingSale.DiscountAmount = sale.DiscountAmount;
                existingSale.FinalAmount = sale.FinalAmount;

                _context.Sales.Update(existingSale);
                await _context.SaveChangesAsync(cancellationToken);

                return existingSale;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the sale in the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the sale.", ex);
            }
        }

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentException("Sale ID must be greater than zero.", nameof(id));

            var sale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);

            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
        public async Task<bool> CancelSaleAsync(int id, CancellationToken cancellationToken)
        {
            var sale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {id} not found.");

            if (sale.IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            sale.IsCancelled = true;

            // Atualiza apenas o campo IsCancelled no banco de dados
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<(List<Sale> Sales, int TotalItems)> GetByCustomerAsync(string customerId, int page = 1, int size = 10, string order = "", CancellationToken cancellationToken = default)
        {
            var query = _context.Sales
                .Where(s => s.CustomerId == customerId)
                .AsQueryable();

            query = Order(order, query);

            int totalItems = await query.CountAsync(cancellationToken);

            var sales = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (sales, totalItems);
        }

        private IQueryable<Sale> Order(string order, IQueryable<Sale> query)
        {
            var validOrderProperties = new Dictionary<string, Expression<Func<Sale, object>>>
            {
                { "salenumber", sale => sale.SaleNumber },
                { "date", sale => sale.Date },
                { "totalamount", sale => sale.TotalAmount },
                { "finalamount", sale => sale.FinalAmount },
                { "discountamount", sale => sale.DiscountAmount },
                { "branchid", sale => sale.BranchId },
                { "iscancelled", sale => sale.IsCancelled }
            };

            if (!string.IsNullOrEmpty(order))
            {
                var orderParams = order.Split(',');
                bool firstOrderApplied = false;

                foreach (var param in orderParams)
                {
                    var orderBy = param.Trim().Split(' ');
                    var property = orderBy[0].ToLower();
                    var direction = orderBy.Length > 1 ? orderBy[1].ToLower() : "asc";

                    if (!validOrderProperties.ContainsKey(property))
                        throw new ArgumentException($"Invalid order property: {property}");

                    var orderExpression = validOrderProperties[property];

                    // Aplica a ordenação inicial ou adiciona uma ordenação secundária
                    if (!firstOrderApplied)
                    {
                        query = direction == "desc" ? query.OrderByDescending(orderExpression) : query.OrderBy(orderExpression);
                        firstOrderApplied = true;
                    }
                    else
                    {
                        query = direction == "desc" ? ((IOrderedQueryable<Sale>)query).ThenByDescending(orderExpression)
                                                    : ((IOrderedQueryable<Sale>)query).ThenBy(orderExpression);
                    }
                }
            }

            return query;
        }

        private async Task<int> GetNextSequenceValueAsync(string sequenceName, CancellationToken cancellationToken)
        {
            var counterCollection = _context.GetCounterCollection();

            var filter = Builders<Counter>.Filter.Eq(c => c.Id, sequenceName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);

            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await counterCollection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
            return counter.SequenceValue;
        }
    }
}
