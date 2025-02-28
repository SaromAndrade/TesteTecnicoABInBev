using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductQueryHandler : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            var result = await _productRepository.DeleteAsync(request.Id, cancellationToken);

            if (!result)
            {
                throw new KeyNotFoundException("Product not found");
            }
            return "Product deleted successfully";
        }
    }
}
