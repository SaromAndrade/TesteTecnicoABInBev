using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB.Seeders.Products
{
    public class ProductFaker : Faker<Product>
    {
        public ProductFaker()
        {
            RuleFor(p => p.Id, f => f.IndexFaker + 1);
            RuleFor(p => p.Title, f => f.Commerce.ProductName());
            RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000));
            RuleFor(p => p.Description, f => f.Lorem.Sentence(10));
            RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0]);
            RuleFor(p => p.Image, f => f.Image.PicsumUrl());
            RuleFor(p => p.Rating, f => new RatingFaker().Generate());
        }
    }
}
