using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerHandler : IRequestHandler<GetSalesByCustomerQuery, GetSalesByCustomerResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesByCustomerHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetSalesByCustomerResult> Handle(GetSalesByCustomerQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetSalesByCustomerQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (paginatedSales, totalItems) = await _saleRepository.GetByCustomerAsync(request.CustomerId, request.Page, request.Size, request.Order, cancellationToken);

            return new GetSalesByCustomerResult { Sales = paginatedSales, TotalItems = totalItems, };
        }
    }
}
