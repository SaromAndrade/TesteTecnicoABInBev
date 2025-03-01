using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB
{
    public class MongoDbContext : DbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _configuration; 
        public MongoDbContext(DbContextOptions<MongoDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("DeveloperEvaluationDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToCollection("products");
            modelBuilder.Entity<Counter>().ToCollection("counters");
            modelBuilder.Entity<Sale>().ToCollection("sales");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public IMongoCollection<Counter> GetCounterCollection() => _database.GetCollection<Counter>("counters");
    }
}
