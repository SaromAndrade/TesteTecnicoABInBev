using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeHandler : IRequestHandler<GetSalesByDateRangeQuery, GetSalesByDateRangeResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesByDateRangeHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetSalesByDateRangeResult> Handle(GetSalesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetSalesByDateRangeQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (sales, totalItems) = await _saleRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, request.Page, request.Size, request.Order, cancellationToken);

            return new GetSalesByDateRangeResult { Sales = sales, TotalItems = totalItems, };
        }
    }
}
