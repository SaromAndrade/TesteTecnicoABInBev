using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid query, When handling, Then returns sale result")]
        public async Task Handle_ValidQuery_ReturnsSaleResult()
        {
            // Arrange
            var query = new GetSaleQuery { Id = 123 };
            var sale = new Sale
            {
                Id = 123,
                SaleNumber = 456,
                Date = DateTime.UtcNow,
                CustomerId = "customer-123",
                TotalAmount = 1000,
                BranchId = 1,
                Products = new List<SaleProduct>
            {
                new SaleProduct
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 500,
                    TotalItemAmount = 1000,
                    DiscountAmount = 100,
                    FinalItemAmount = 900
                }
            },
                IsCancelled = false,
                FinalAmount = 900,
                DiscountAmount = 100
            };
            var result = new GetSaleResult
            {
                Id = 123,
                SaleNumber = 456,
                Date = sale.Date,
                CustomerId = "customer-123",
                BranchId = "1",
                Products = sale.Products,
                IsCancelled = false,
            };

            _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns(sale);
            _mapper.Map<GetSaleResult>(sale).Returns(result);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetByIdAsync(query.Id, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<GetSaleResult>(sale);
            response.Should().BeEquivalentTo(result);
        }

        [Fact(DisplayName = "Given invalid query (Id <= 0), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQueryId_ThrowsValidationException()
        {
            // Arrange
            var query = new GetSaleQuery { Id = 0 }; // ID inválido
            var validator = new GetSaleQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var query = new GetSaleQuery { Id = 123 };
            _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error fetching sale"));

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error fetching sale");
        }
    }
}
