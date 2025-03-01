using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesQuery : IRequest<GetAllSalesResult>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }
    }
}
