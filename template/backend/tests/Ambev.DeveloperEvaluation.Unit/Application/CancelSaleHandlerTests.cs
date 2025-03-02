using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CancelSaleHandler _handler;

        public CancelSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CancelSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given valid command, When handling, Then cancels sale and returns success message")]
        public async Task Handle_ValidCommand_CancelsSaleAndReturnsSuccessMessage()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 123 };
            _saleRepository.CancelSaleAsync(command.SaleNumber, Arg.Any<CancellationToken>()).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).CancelSaleAsync(command.SaleNumber, Arg.Any<CancellationToken>());
            result.Should().Be("Sale canceled successfully");
        }

        [Fact(DisplayName = "Given invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 0 }; // SaleNumber inválido
            var validator = new CancelSaleCommandValidator();
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
            var command = new CancelSaleCommand { SaleNumber = 123 };
            _saleRepository.CancelSaleAsync(command.SaleNumber, Arg.Any<CancellationToken>()).Returns(false);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Sale not found");
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 123 };
            _saleRepository.CancelSaleAsync(command.SaleNumber, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error canceling sale"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error canceling sale");
        }
    }
}
