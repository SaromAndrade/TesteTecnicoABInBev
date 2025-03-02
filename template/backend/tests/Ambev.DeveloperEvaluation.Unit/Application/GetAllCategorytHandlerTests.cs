using Ambev.DeveloperEvaluation.Application.Products.GetAllCategory;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetAllCategorytHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly GetAllCategorytHandler _handler;

        public GetAllCategorytHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetAllCategorytHandler(_productRepository);
        }

        [Fact(DisplayName = "Given categories in repository, When handling, Then returns categories list")]
        public async Task Handle_ValidCategories_ReturnsCategoriesList()
        {
            // Arrange
            var expectedCategories = new List<string> { "Electronics", "Clothing", "Books" };
            var command = new GetAllCategoryQuery();

            // Simulando o repositório para retornar categorias
            _productRepository.GetAllCategoriesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedCategories));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Categories.Should().BeEquivalentTo(expectedCategories);
        }

        [Fact(DisplayName = "Given no categories in repository, When handling, Then returns empty categories list")]
        public async Task Handle_NoCategories_ReturnsEmptyCategoriesList()
        {
            // Arrange
            var expectedCategories = new List<string>();
            var command = new GetAllCategoryQuery();

            // Simulando o repositório para retornar uma lista vazia
            _productRepository.GetAllCategoriesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedCategories));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Categories.Should().BeEmpty();
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new GetAllCategoryQuery();

            // Simulando que o repositório lança uma exceção
            _productRepository.GetAllCategoriesAsync(Arg.Any<CancellationToken>()).Throws(new Exception("Error fetching categories"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Error fetching categories");
        }
    }
}
