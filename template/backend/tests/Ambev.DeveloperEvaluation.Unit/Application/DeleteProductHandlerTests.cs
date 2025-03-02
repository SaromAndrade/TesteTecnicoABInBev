using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new DeleteProductHandler(_productRepository);
        }

        [Fact(DisplayName = "Given a non-existent product, When handling, Then throws KeyNotFoundException")]
        public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 1;
            var command = new DeleteProductCommand { Id = productId };

            // Simulando que o produto não existe
            _productRepository.DeleteAsync(productId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(false));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product not found");
        }

        [Fact(DisplayName = "Given an existing product, When handling, Then returns success message")]
        public async Task Handle_ProductDeletedSuccessfully_ReturnsSuccessMessage()
        {
            // Arrange
            var productId = 1;
            var command = new DeleteProductCommand { Id = productId };
            var successMessage = "Product deleted successfully";

            // Simulando que o produto existe e pode ser deletado
            _productRepository.DeleteAsync(productId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(successMessage);
        }

        [Fact(DisplayName = "Given an invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new DeleteProductCommand(); // Comando inválido (Id não foi preenchido)
            var validator = new DeleteProductQueryValidator();
            var validationResult = await validator.ValidateAsync(command);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Act & Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given an existing product, When delete operation fails, Then throws exception")]
        public async Task Handle_DeleteOperationFails_ThrowsException()
        {
            // Arrange
            var productId = 1;
            var command = new DeleteProductCommand { Id = productId };

            // Simulando que a operação de deletar o produto falhou
            _productRepository.DeleteAsync(productId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(false));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product not found");
        }
    }
}
