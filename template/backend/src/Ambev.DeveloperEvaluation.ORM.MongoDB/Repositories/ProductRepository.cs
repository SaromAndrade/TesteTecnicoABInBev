using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver.Linq;

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
        public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        }
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all products.</returns>
        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(string id, Product product, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (existingProduct != null)
            {
                existingProduct.Title = product.Title;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Image = product.Image;
                existingProduct.Rating = product.Rating;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
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
    }
}
