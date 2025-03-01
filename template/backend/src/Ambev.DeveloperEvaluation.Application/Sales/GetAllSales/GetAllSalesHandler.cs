using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, GetAllSalesResult>
    {
        private readonly ISaleRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetAllSalesHandler(ISaleRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSalesResult> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetAllSalesQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (paginatedSales, totalItems) = await _cartRepository.GetAllAsync(request.Page, request.Size, request.Order, cancellationToken);
            var paginatedSalesResult = _mapper.Map<List<Sale>>(paginatedSales);

            return new GetAllSalesResult { Sales = paginatedSalesResult, TotalItems = totalItems,  };
        }
    }
}
