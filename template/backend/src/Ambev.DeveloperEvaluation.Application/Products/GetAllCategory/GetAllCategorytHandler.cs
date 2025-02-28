using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllCategory
{
    public class GetAllCategorytHandler : IRequestHandler<GetAllCategoryQuery, GetAllCategorytResult>
    {
        private readonly IProductRepository _productRepository;

        public GetAllCategorytHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetAllCategorytResult> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var list = await _productRepository.GetAllCategoriesAsync(cancellationToken);
            return new GetAllCategorytResult { Categories = list};
        }
    }
}
