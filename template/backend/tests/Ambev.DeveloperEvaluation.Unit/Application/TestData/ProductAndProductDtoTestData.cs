using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class ProductAndProductDtoTestData
    {
        private static readonly Faker<Product> productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.Rating, f => new Rating { Rate = f.Random.Double(1, 5), Count = f.Random.Int(1, 100) });

        private static readonly Faker<ProductDto> productDtosFaker = new Faker<ProductDto>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 100).ToString())
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.Rating, f => new RatingDto { Rate = f.Random.Double(1, 5), Count = f.Random.Int(1, 100) });

        public static List<Product> GenerateValidProduct(int qtd) => productFaker.Generate(qtd);
        public static List<ProductDto> GenerateValidProductDto(int qtd) => productDtosFaker.Generate(qtd);
        public static Product GenerateValidProduct() => productFaker.Generate();
        public static ProductDto GenerateValidProductDto() => productDtosFaker.Generate();
    }
}
