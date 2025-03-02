using Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Products;
using Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Sales;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Threading;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(MongoDbContext context)
        {
            context.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            int nextSaleId = 1; 
            int nextProductId = 1; 

            if (!context.Products.Any())
            {
                var faker = new ProductFaker();
                var products = faker.Generate(10);

                if (products.Any())
                {
                    nextProductId = products.Max(p => p.Id);
                }

                await context.Products.AddRangeAsync(products);
            }
            if (!context.Sales.Any())
            {
                var faker = new SaleFaker();
                var sales = faker.Generate(10);

                if (sales.Any())
                {
                    nextSaleId = sales.Max(s => s.Id);
                }

                await context.Sales.AddRangeAsync(sales);
            }

            var lastSale = await context.Sales.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            var lastProduct = await context.Products.OrderByDescending(s => s.Id).FirstOrDefaultAsync();

            // Pega o maior ID entre o último ID encontrado na tabela e o próximo valor calculado
            int finalSaleId = lastSale?.Id > nextSaleId ? lastSale.Id : nextSaleId;
            int finalProductId = lastProduct?.Id > nextProductId ? lastProduct.Id : nextProductId;

            await context.Counters.AddAsync(new Domain.Entities.Counter { Id = "saleid", SequenceValue = finalSaleId });
            await context.Counters.AddAsync(new Domain.Entities.Counter { Id = "productid", SequenceValue = finalProductId });

            await context.SaveChangesAsync();
        }
    }
}
