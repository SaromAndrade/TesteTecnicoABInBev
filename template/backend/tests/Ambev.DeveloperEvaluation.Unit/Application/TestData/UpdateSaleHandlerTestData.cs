using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public class UpdateSaleHandlerTestData
    {
        private static readonly Faker<UpdateSaleCommand> faker = new Faker<UpdateSaleCommand>()
        .RuleFor(c => c.Id, f => f.Random.Int(1, 1000)) // Gera um ID aleatório
        .RuleFor(c => c.CustomerId, f => f.Random.Guid().ToString()) // Gera um CustomerId aleatório
        .RuleFor(c => c.Products, f => GenerateSaleProducts(f)) // Gera uma lista de produtos
        .RuleFor(c => c.IsCancelled, f => f.Random.Bool()); // Gera um valor booleano aleatório para IsCancelled

        private static List<SaleProductDto> GenerateSaleProducts(Faker f)
        {
            var products = new List<SaleProductDto>();
            for (int i = 0; i < f.Random.Int(1, 5); i++) // Gera entre 1 e 5 produtos
            {
                products.Add(new SaleProductDto
                {
                    ProductId = f.Random.Int(1, 1000), // Gera um ProductId entre 1 e 1000
                    Quantity = f.Random.Int(1, 20), // Gera uma quantidade entre 1 e 20
                    UnitPrice = f.Random.Decimal(10, 1000) // Gera um preço unitário entre 10 e 1000
                });
            }
            return products;
        }
        public static UpdateSaleCommand GenerateValidUpdateSaleCommand() => faker.Generate();

    }
}
