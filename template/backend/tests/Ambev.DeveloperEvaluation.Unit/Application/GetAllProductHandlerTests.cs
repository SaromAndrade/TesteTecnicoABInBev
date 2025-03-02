using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
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
    public class GetAllProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly GetAllProductHandler _handler;
        private readonly IMapper _mapper;

        public GetAllProductHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetAllProductHandler(_productRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid query, When handling, Then returns paginated product data")]
        public async Task Handle_ValidQuery_ReturnsPaginatedProductData()
        {
            var products = ProductAndProductDtoTestData.GenerateValidProduct(10);
            var productDtos = ProductAndProductDtoTestData.GenerateValidProductDto(10);

            var command = new GetAllProductQuery { Page = 1, Size = 10 };
            // Simulando o repositório para retornar produtos paginados
            _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((products, products.Count)));

            _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Data.Should().BeEquivalentTo(productDtos);
            result.TotalItems.Should().Be(10);
            result.TotalPages.Should().Be(1);
            result.CurrentPage.Should().Be(1);
        }

        [Fact(DisplayName = "Given invalid query, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQuery_ThrowsValidationException()
        {
            // Arrange
            var command = new GetAllProductQuery { Page = -1, Size = 0 }; // Comando com parâmetros inválidos
            var validator = new GetAllProductQueryValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given empty repository, When handling, Then returns empty data")]
        public async Task Handle_EmptyRepository_ReturnsEmptyData()
        {
            // Arrange
            var command = new GetAllProductQuery { Page = 1, Size = 10, Order = "title desc" };

            // Simulando o repositório para retornar uma lista vazia de produtos
            _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((new List<Product>(), 0)));
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
            var command = new GetAllProductQuery { Page = 1, Size = 10 };

            // Simulando que o repositório lança uma exceção
            _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Error fetching products"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Error fetching products");
        }
    }
}
