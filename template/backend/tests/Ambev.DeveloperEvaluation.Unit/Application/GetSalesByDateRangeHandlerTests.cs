using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange;
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
    public class GetSalesByDateRangeHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSalesByDateRangeHandler _handler;

        public GetSalesByDateRangeHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSalesByDateRangeHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid query, When handling, Then returns paginated sales by date range")]
        public async Task Handle_ValidQuery_ReturnsPaginatedSalesByDateRange()
        {
            // Arrange
            var query = new GetSalesByDateRangeQuery
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Page = 1,
                Size = 10,
                Order = "date desc"
            };
            var paginatedSales = new List<Sale>
            {
                new Sale { Id = 1, SaleNumber = 123, Date = DateTime.UtcNow }
            };
            var totalItems = 1;

            _saleRepository.GetByDateRangeAsync(query.StartDate, query.EndDate, query.Page, query.Size, query.Order, Arg.Any<CancellationToken>())
                .Returns((paginatedSales, totalItems));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).GetByDateRangeAsync(
                query.StartDate, query.EndDate, query.Page, query.Size, query.Order, Arg.Any<CancellationToken>());
            result.Sales.Should().BeEquivalentTo(paginatedSales);
            result.TotalItems.Should().Be(totalItems);
        }

        [Fact(DisplayName = "Given invalid query (StartDate > EndDate), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQueryStartDate_ThrowsValidationException()
        {
            // Arrange
            var query = new GetSalesByDateRangeQuery
            {
                StartDate = DateTime.UtcNow, // StartDate maior que EndDate
                EndDate = DateTime.UtcNow.AddDays(-7),
                Page = 1,
                Size = 10,
                Order = "date desc"
            };
            var validator = new GetSalesByDateRangeQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given invalid query (Page <= 0), When handling, Then throws ValidationException")]
        public async Task Handle_InvalidQueryPage_ThrowsValidationException()
        {
            // Arrange
            var query = new GetSalesByDateRangeQuery
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Page = 0, // Page inválido
                Size = 10,
                Order = "date desc"
            };
            var validator = new GetSalesByDateRangeQueryValidator();
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
            var query = new GetSalesByDateRangeQuery
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Page = 1,
                Size = 101, // Size inválido
                Order = "date desc"
            };
            var validator = new GetSalesByDateRangeQueryValidator();
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
            var query = new GetSalesByDateRangeQuery
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Page = 1,
                Size = 10,
                Order = "date desc"
            };
            _saleRepository.GetByDateRangeAsync(query.StartDate, query.EndDate, query.Page, query.Size, query.Order, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error fetching sales"));

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error fetching sales");
        }
    }
}
