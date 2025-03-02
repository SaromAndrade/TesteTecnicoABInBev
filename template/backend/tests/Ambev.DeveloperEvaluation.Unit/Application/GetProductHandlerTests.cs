using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
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
    public class GetProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly GetProductHandler _handler;
        private readonly IMapper _mapper;

        public GetProductHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetProductHandler(_productRepository, _mapper);
        }

        [Fact(DisplayName = "Given a valid product ID, When handling, Then returns the product result")]
        public async Task Handle_ValidProductId_ReturnsProductResult()
        {
            // Arrange
            var command = new GetProductQuery { Id = 1 };
            var product = ProductAndProductDtoTestData.GenerateValidProduct();

            var productResult = new GetProductResult
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Image = product.Image,
                Rating = new RatingDto { Rate = 4.5, Count = 10 }
            };

            // Mocking the repository
            _productRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(product));

            // Mocking the mapper
            _mapper.Map<GetProductResult>(Arg.Any<Product>())
                .Returns(productResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(productResult);
        }

        [Fact(DisplayName = "Given an invalid product ID, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidProductId_ThrowsValidationException()
        {
            // Arrange
            var command = new GetProductQuery { Id = 0 };  // Um ID inválido (ex: 0)
            var validator = new GetProductQueryValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given a product not found in the repository, When handling, Then returns null result")]
        public async Task Handle_ProductNotFound_ReturnsNull()
        {
            // Arrange
            var command = new GetProductQuery { Id = 1 };

            // Simulando o repositório para retornar null (produto não encontrado)
            _productRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new GetProductQuery { Id = 1 };

            // Simulando o repositório lançar uma exceção
            _productRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Database error"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
        [Fact(DisplayName = "Given valid product ID, When handling, Then calls repository and mapper")]
        public async Task Handle_ValidProductId_CallsRepositoryAndMapper()
        {
            // Arrange
            var command = new GetProductQuery { Id = 1 };
            var product = ProductAndProductDtoTestData.GenerateValidProduct();

            // Mocking the repository to return a product
            _productRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(product));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            // Verificando se o repositório e o mapeador foram chamados
            await _productRepository.Received().GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            _mapper.Received().Map<GetProductResult>(product);
        }
    }
}
