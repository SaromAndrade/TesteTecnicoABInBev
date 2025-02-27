using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB
{
    public class MongoDbContext : DbContext
    {
        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToCollection("products");
        }
        public DbSet<Product> Products { get; set; }
    }
}
