using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    /// <summary>
    /// Handler for the <see cref="CreateProductCommand"/>.
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the creation of a new product.
        /// </summary>
        /// <param name="request">The create product command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response containing the created product details.</returns>
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = _mapper.Map<Product>(request);

            var createdProduct =  await _productRepository.CreateAsync(product, cancellationToken);
            // Mapeia o produto criado para a resposta
            var result = _mapper.Map<CreateProductResult>(createdProduct);
            return result;
        }
    }
}
