using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
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
    public class CreateProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateProductHandler(_productRepository, _mapper);
        }

        [Fact(DisplayName = "Given a valid command, When handling, Then creates a product and returns expected result")]
        public async Task Handle_ValidCommand_CreatesProductAndReturnsResult()
        {
            // Arrange
            var command = CreateProductHandlerTestData.GenerateValidCommand();
            var product = new Product 
            { 
                Id = 1,
                Title = command.Title,
                Price = command.Price,
                Category = command.Category,
                Description = command.Description,
                Image = command.Image,
            };
            var expectedResult = new CreateProductResult { Id = product.Id, Title = product.Title, Price = product.Price };

            _mapper.Map<Product>(command).Returns(product);
            _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(product);
            _mapper.Map<CreateProductResult>(product).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mapper.Received(1).Map<Product>(command);
            await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<CreateProductResult>(product);
        }

        [Fact(DisplayName = "Given an invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateProductCommand(); // Comando vazio será inválido
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given repository fails, When handling, Then propagates exception")]
        public async Task Handle_RepositoryFailure_ThrowsException()
        {
            // Arrange
            var command = CreateProductHandlerTestData.GenerateValidCommand();
            var product = new Product { Id = 1, Title = command.Title, Price = command.Price };

            _mapper.Map<Product>(command).Returns(product);
            _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
    }
}
