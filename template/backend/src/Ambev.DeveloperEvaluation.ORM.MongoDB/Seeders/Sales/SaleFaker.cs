using Bogus;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Sales
{
    public class SaleFaker : Faker<Sale>
    {
        public SaleFaker()
        {
            RuleFor(s => s.Id, f => f.IndexFaker + 1);
            RuleFor(s => s.SaleNumber, f => f.Random.Int(1000, 9999));
            RuleFor(s => s.Date, f => f.Date.Between(DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow));
            RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString());
            RuleFor(s => s.BranchId, f => f.Random.Int(1, 10));
            RuleFor(s => s.Products, f => new SaleProductFaker().Generate(f.Random.Int(1, 5)));
            RuleFor(s => s.IsCancelled, f => f.Random.Bool(0.1f));

            RuleFor(s => s.TotalAmount, (f, s) => s.Products.Sum(p => p.TotalItemAmount));
            RuleFor(s => s.DiscountAmount, (f, s) => s.Products.Sum(p => p.DiscountAmount));
            RuleFor(s => s.FinalAmount, (f, s) => s.TotalAmount - s.DiscountAmount);
        }
    }
}
