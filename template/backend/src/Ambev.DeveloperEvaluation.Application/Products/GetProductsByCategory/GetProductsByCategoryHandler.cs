using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetProductsByCategoryQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (paginatedProducts, totalItems) = await _productRepository.GetByCategoryAsync(request.Category, request.Page, request.Size, request.Order, cancellationToken);
            var productDtos = _mapper.Map<List<ProductDto>>(paginatedProducts);
            int totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

            return new GetProductsByCategoryResult { CurrentPage = request.Page, TotalItems = totalItems, Data = productDtos, TotalPages = totalPages };
        }
    }
}
