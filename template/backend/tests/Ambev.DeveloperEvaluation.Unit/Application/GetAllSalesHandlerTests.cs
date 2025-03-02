using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
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
    public class GetAllSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetAllSalesHandler _handler;

        public GetAllSalesHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllSalesHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid query, When handling, Then returns paginated sales")]
        public async Task Handle_ValidQuery_ReturnsPaginatedSales()
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = 1, Size = 10, Order = "date desc" };
            var paginatedSales = new List<Sale> { new Sale { Id = 1, SaleNumber = 123, Date = DateTime.UtcNow } };
            var totalItems = 1;
            var mappedSales = new List<Sale> { new Sale { Id = 1, SaleNumber = 123, Date = DateTime.UtcNow } };

            _saleRepository.GetAllAsync(query.Page, query.Size, query.Order, Arg.Any<CancellationToken>())
                .Returns((paginatedSales, totalItems));
            _mapper.Map<List<Sale>>(paginatedSales).Returns(mappedSales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetAllAsync(query.Page, query.Size, query.Order, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<List<Sale>>(paginatedSales);
            result.Sales.Should().BeEquivalentTo(mappedSales);
            result.TotalItems.Should().Be(totalItems);
        }

        [Fact(DisplayName = "Given invalid query (Page <= 0), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQueryPage_ThrowsValidationException()
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = 0, Size = 10, Order = "date desc" }; // Page inválido
            var validator = new GetAllSalesQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given invalid query (Size > 100), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQuerySize_ThrowsValidationException()
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = 1, Size = 101, Order = "date desc" }; // Size inválido
            var validator = new GetAllSalesQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given invalid query (invalid Order expression), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQueryOrder_ThrowsValidationException()
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = 1, Size = 10, Order = "invalidField asc" }; // Order inválido
            var validator = new GetAllSalesQueryValidator();
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
            var query = new GetAllSalesQuery { Page = 1, Size = 10, Order = "date desc" };
            _saleRepository.GetAllAsync(query.Page, query.Size, query.Order, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error fetching sales"));

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error fetching sales");
        }
    }
}
