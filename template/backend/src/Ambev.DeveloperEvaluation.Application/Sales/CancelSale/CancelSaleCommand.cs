using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommand : IRequest<string>
    {
        public int SaleNumber { get; set; }
    }
}
