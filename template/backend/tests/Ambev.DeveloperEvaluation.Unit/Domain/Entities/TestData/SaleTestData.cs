using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<SaleProduct> SaleProductFaker = new Faker<SaleProduct>()
            .RuleFor(p => p.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(p => p.UnitPrice, f => f.Finance.Amount(5, 500))
            .RuleFor(p => p.TotalItemAmount, (f, p) => p.Quantity * p.UnitPrice)
            .RuleFor(p => p.DiscountAmount, f => f.Finance.Amount(0, 50))
            .RuleFor(p => p.FinalItemAmount, (f, p) => p.TotalItemAmount - p.DiscountAmount);

        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.SaleNumber, f => f.Random.Int(1000, 9999))
            .RuleFor(s => s.Date, f => f.Date.Past(1))
            .RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString())
            .RuleFor(s => s.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(s => s.Products, f => SaleProductFaker.Generate(f.Random.Int(1, 5)))
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool(0.1f))
            .RuleFor(s => s.TotalAmount, (f, s) => s.Products.Sum(p => p.TotalItemAmount))
            .RuleFor(s => s.DiscountAmount, f => f.Finance.Amount(0, 100))
            .RuleFor(s => s.FinalAmount, (f, s) => s.TotalAmount - s.DiscountAmount);

        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

    }
}
