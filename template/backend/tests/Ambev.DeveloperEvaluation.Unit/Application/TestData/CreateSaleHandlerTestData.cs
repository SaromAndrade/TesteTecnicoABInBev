using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> faker = new Faker<CreateSaleCommand>()
        .RuleFor(c => c.CustomerId, f => f.Random.Guid().ToString())
        .RuleFor(c => c.BranchId, f => f.Random.Int(1, 100))
        .RuleFor(c => c.Products, f => GenerateSaleProducts(f));

        private static List<SaleProductDto> GenerateSaleProducts(Faker f)
        {
            var products = new List<SaleProductDto>();

            products.Add(new SaleProductDto
            {
                ProductId = f.Random.Int(1, 1000),
                Quantity = f.Random.Int(1, 20), 
                UnitPrice = f.Random.Decimal(10, 1000)
            });
            return products;
        }
        public static CreateSaleCommand GenerateValidSaleCommand() => faker.Generate();

    }
}
