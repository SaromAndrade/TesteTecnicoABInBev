using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetProductsByCategoryHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly GetProductsByCategoryHandler _handler;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetProductsByCategoryHandler(_productRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid query, When handling, Then returns the products in category")]
        public async Task Handle_ValidQuery_ReturnsProductsInCategory()
        {
            // Arrange
            var command = new GetProductsByCategoryQuery
            {
                Category = "Electronics",
                Page = 1,
                Size = 10,
                Order = "title desc"
            };

            var products = ProductAndProductDtoTestData.GenerateValidProduct(2);
            var productDtos = ProductAndProductDtoTestData.GenerateValidProductDto(2);

            // Mocking repository
            _productRepository.GetByCategoryAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((products, 2)));

            // Mocking mapper
            _mapper.Map<List<ProductDto>>(Arg.Any<List<Product>>())
                .Returns(productDtos);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Data.Should().BeEquivalentTo(productDtos);
            result.TotalItems.Should().Be(2);
            result.TotalPages.Should().Be(1);
            result.CurrentPage.Should().Be(1);
        }

        [Fact(DisplayName = "Given invalid query, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQuery_ThrowsValidationException()
        {
            // Arrange
            var command = new GetProductsByCategoryQuery
            {
                Category = "",  // Categoria inválida
                Page = 1,
                Size = 10,
                Order = "title desc"
            };

            var validator = new GetProductsByCategoryQueryValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given no products in category, When handling, Then returns empty data")]
        public async Task Handle_NoProductsInCategory_ReturnsEmptyData()
        {
            // Arrange
            var command = new GetProductsByCategoryQuery
            {
                Category = "NonExistentCategory",
                Page = 1,
                Size = 10,
                Order = "title desc"
            };

            // Mocking repository to return empty list
            _productRepository.GetByCategoryAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((new List<Product>(), 0)));

            // Mocking mapper
            _mapper.Map<List<ProductDto>>(Arg.Any<List<Product>>())
                .Returns(new List<ProductDto>());
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Data.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.CurrentPage.Should().Be(1);
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new GetProductsByCategoryQuery { Category = "Electronics" };

            // Simulando o repositório lançar uma exceção
            _productRepository.GetByCategoryAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Database error"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
    }
}
