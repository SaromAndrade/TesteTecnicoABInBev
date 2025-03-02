using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Products
{
    public class RatingFaker : Faker<Rating>
    {
        public RatingFaker()
        {
            RuleFor(r => r.Rate, f => f.Random.Double(1.0, 5.0));
            RuleFor(r => r.Count, f => f.Random.Int(0, 500));
        }
    }
}
