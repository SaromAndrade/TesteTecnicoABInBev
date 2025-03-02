using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new UpdateSaleHandler(_saleRepository, _mapper, _productRepository);
        }


        [Fact(DisplayName = "Given valid command, When handling, Then updates sale and returns result")]
        public async Task Handle_ValidCommand_UpdatesSaleAndReturnsResult()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidUpdateSaleCommand();
            var sale = new Sale()
            {
                Id = command.Id,
                IsCancelled = command.IsCancelled,
                CustomerId = command.CustomerId,
                Products = new List<SaleProduct>() {
                    new SaleProduct() {
                        ProductId = command.Products[0].ProductId,
                        Quantity = command.Products[0].Quantity,
                        UnitPrice = command.Products[0].UnitPrice,
                    }
                },
            };
            var updatedSale = new Sale();
            var result = new UpdateSaleResult();

            // Mockando o IProductRepository para retornar true para todos os ProductIds
            foreach (var product in command.Products)
            {
                _productRepository.ExistsAsync(product.ProductId, Arg.Any<CancellationToken>()).Returns(true);
            }

            _mapper.Map<Sale>(command).Returns(sale);
            _saleRepository.UpdateAsync(command.Id, sale, Arg.Any<CancellationToken>()).Returns(updatedSale);
            _mapper.Map<UpdateSaleResult>(updatedSale).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).UpdateAsync(command.Id, sale, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<UpdateSaleResult>(updatedSale);
            response.Should().Be(result);
        }

        [Fact(DisplayName = "Given invalid command (ProductId does not exist), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidProductId_ThrowsValidationException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidUpdateSaleCommand();

            // Mockando o IProductRepository para retornar false para o primeiro ProductId
            _productRepository.ExistsAsync(command.Products[0].ProductId, Arg.Any<CancellationToken>()).Returns(false);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*Product with ID {command.Products[0].ProductId} does not exist.*");
        }

        [Fact(DisplayName = "Given invalid command (Quantity > 20), When handling, Then throws ArgumentException")]
        public async Task Handle_InvalidQuantity_ThrowsArgumentException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidUpdateSaleCommand();
            command.Products[0].Quantity = 21;

            var sale = new Sale()
            {
                Id = command.Id,
                IsCancelled = command.IsCancelled,
                CustomerId = command.CustomerId,
                Products = new List<SaleProduct>() {
                    new SaleProduct() {
                        ProductId = command.Products[0].ProductId,
                        Quantity = command.Products[0].Quantity,
                        UnitPrice = command.Products[0].UnitPrice,
                    }
                },
            };

            // Mockando o IProductRepository para retornar true para todos os ProductIds
            foreach (var product in command.Products)
            {
                _productRepository.ExistsAsync(product.ProductId, Arg.Any<CancellationToken>()).Returns(true);
            }

            _mapper.Map<Sale>(command).Returns(sale);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"*Produto {command.Products[0].ProductId} não pode ter mais de 20 unidades por venda.*");
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidUpdateSaleCommand();

            var sale = new Sale()
            {
                Id = command.Id,
                IsCancelled = command.IsCancelled,
                CustomerId = command.CustomerId,
                Products = new List<SaleProduct>() {
                    new SaleProduct() {
                        ProductId = command.Products[0].ProductId,
                        Quantity = command.Products[0].Quantity,
                        UnitPrice = command.Products[0].UnitPrice,
                    }
                },
            };

            // Mockando o IProductRepository para retornar true para todos os ProductIds
            foreach (var product in command.Products)
            {
                _productRepository.ExistsAsync(product.ProductId, Arg.Any<CancellationToken>()).Returns(true);
            }

            _mapper.Map<Sale>(command).Returns(sale);
            _saleRepository.UpdateAsync(command.Id, Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Error updating sale"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error updating sale");
        }
    }
}
