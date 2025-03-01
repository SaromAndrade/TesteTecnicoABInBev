using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}
