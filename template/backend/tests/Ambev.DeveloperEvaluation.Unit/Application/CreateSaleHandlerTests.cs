using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper, _productRepository);
        }

        [Fact(DisplayName = "Given valid command, When handling, Then creates sale and returns result")]
        public async Task Handle_ValidCommand_CreatesSaleAndReturnsResult()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            var sale = new Sale();
            var createdSale = new Sale();
            var result = new CreateSaleResult();

            foreach (var product in command.Products)
            {
                _productRepository.ExistsAsync(product.ProductId, Arg.Any<CancellationToken>()).Returns(true);
            }

            _mapper.Map<Sale>(command).Returns(sale);
            _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(createdSale);
            _mapper.Map<CreateSaleResult>(createdSale).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<CreateSaleResult>(createdSale);
            response.Should().Be(result);
        }

        [Fact(DisplayName = "Given invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                CustomerId = "", // CustomerId inválido
                BranchId = 0, // BranchId inválido
                Products = new List<SaleProductDto>() // Lista de produtos vazia
            };


            var validator = new CreateSaleCommandValidator(_productRepository);
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }
        [Fact(DisplayName = "Given product quantity exceeds limit, When handling, Then throws ArgumentException")]
        public async Task Handle_ProductQuantityExceedsLimit_ThrowsArgumentException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            command.Products[0].Quantity = 21;

            var sale = new Sale()
            {
                BranchId = command.BranchId,
                CustomerId = command.CustomerId,
                Products = new List<SaleProduct>() {
                    new SaleProduct() {
                        ProductId = command.Products[0].ProductId,
                        Quantity = command.Products[0].Quantity,
                        UnitPrice = command.Products[0].UnitPrice,
                    }
                },
            };


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
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();

            foreach (var product in command.Products)
            {
                _productRepository.ExistsAsync(product.ProductId, Arg.Any<CancellationToken>()).Returns(true);
            }

            _mapper.Map<Sale>(command).Returns(new Sale());
            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Error creating sale"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error creating sale");
        }
    }
}
