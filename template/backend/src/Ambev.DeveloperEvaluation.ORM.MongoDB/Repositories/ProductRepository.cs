using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Repositories
{
    /// <summary>
    /// Repository for managing products in the database.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ProductRepository(MongoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<Product?> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            product.Id = await GetNextSequenceValueAsync("productid", cancellationToken);
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync();
            return product;
        }
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        }
        /// <summary>
        /// Retrieves a paginated list of products from the database.
        /// </summary>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="size">The number of items per page (default is 10).</param>
        /// <param name="order">Sorting order (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A paginated list of products.</returns>
        public async Task<(List<Product> Products, int TotalItems)> GetAllAsync(int page = 1, int size = 10, string order = "", CancellationToken cancellationToken = default)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(order))
            {
                var orderParams = order.Split(',');
                foreach (var param in orderParams)
                {
                    var orderBy = param.Trim().Split(' ');
                    var property = orderBy[0];
                    var direction = orderBy.Length > 1 ? orderBy[1] : "asc";

                    switch (property.ToLower())
                    {
                        case "title":
                            query = direction.ToLower() == "desc"
                                ? query.OrderByDescending(p => p.Title)
                                : query.OrderBy(p => p.Title);
                            break;
                        case "price":
                            query = direction.ToLower() == "desc"
                                ? query.OrderByDescending(p => p.Price)
                                : query.OrderBy(p => p.Price);
                            break;
                        case "category":
                            query = direction.ToLower() == "desc"
                                ? query.OrderByDescending(p => p.Category)
                                : query.OrderBy(p => p.Category);
                            break;
                        // Adicione mais casos para outras propriedades, se necessário
                        default:
                            throw new ArgumentException($"Invalid order property: {property}");
                    }
                }
            }
            int totalItems = await _context.Products.CountAsync(cancellationToken);
            var products = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);

            return (products, totalItems);
        }
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<Product> UpdateAsync(int id, Product product, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));

            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product data cannot be null.");

            try
            {
                var existingProduct = await _context.Products.FindAsync(new object[] { id }, cancellationToken);

                if (existingProduct == null)
                    throw new KeyNotFoundException($"Product with ID {id} not found.");

                // Update product fields
                existingProduct.Title = product.Title;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Image = product.Image;
                existingProduct.Rating = product.Rating;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync(cancellationToken);

                return existingProduct;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the product in the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the product.", ex);
            }
        }
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Retrieves products by category.
        /// </summary>
        /// <param name="category">The category to filter products by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of products in the specified category.</returns>
        public async Task<List<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync(cancellationToken);
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
