using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, GetAllProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetAllProductResult> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetAllProductQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Processa a consulta
            var (paginatedProducts, totalItems) = await _productRepository.GetAllAsync(request.Page, request.Size, request.Order, cancellationToken);
            var productDtos = _mapper.Map<List<ProductDto>>(paginatedProducts);
            int totalPages = (int)Math.Ceiling((double)totalItems / request.Size);
            
            return new GetAllProductResult { CurrentPage = request.Page, Data = productDtos, TotalItems = productDtos.Count, TotalPages = totalPages };
        }
    }
}
