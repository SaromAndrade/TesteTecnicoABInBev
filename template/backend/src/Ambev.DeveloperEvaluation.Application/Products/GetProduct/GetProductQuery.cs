using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQuery : IRequest<GetProductResult>
    {
        public string Id { get; set; }
    }
}
