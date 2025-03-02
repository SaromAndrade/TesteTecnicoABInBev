using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Sales
{
    public class SaleProductFaker : Faker<SaleProduct>
    {
        public SaleProductFaker()
        {
            RuleFor(p => p.ProductId, f => f.Random.Int(1, 100));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 10));
            RuleFor(p => p.UnitPrice, f => f.Finance.Amount(5, 500));
            RuleFor(p => p.DiscountAmount, f => f.Finance.Amount(0, 50));
            RuleFor(p => p.TotalItemAmount, (f, p) => p.Quantity * p.UnitPrice);
            RuleFor(p => p.FinalItemAmount, (f, p) => p.TotalItemAmount - p.DiscountAmount);
        }
    }
}
