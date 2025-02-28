using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductCommand : IRequest<string>
    {
        public int Id { get; set; }

    }
}
