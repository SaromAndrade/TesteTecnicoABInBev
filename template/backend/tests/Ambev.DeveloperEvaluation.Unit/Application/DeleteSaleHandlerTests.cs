using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given valid command, When handling, Then deletes sale and returns success message")]
        public async Task Handle_ValidCommand_DeletesSaleAndReturnsSuccessMessage()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = 123 };
            _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
            result.Should().Be("Sale deleted successfully");
        }
        [Fact(DisplayName = "Given invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = 0 }; // ID inválido
            var validator = new DeleteSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }
        [Fact(DisplayName = "Given sale not found, When handling, Then throws KeyNotFoundException")]
        public async Task Handle_SaleNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = 123 };
            _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Product not found");
        }
        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = 123 };
            _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error deleting sale"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error deleting sale");
        }
    }
}
