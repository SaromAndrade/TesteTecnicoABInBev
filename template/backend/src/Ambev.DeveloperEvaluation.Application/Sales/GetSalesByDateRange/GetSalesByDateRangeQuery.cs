using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeQuery : IRequest<GetSalesByDateRangeResult>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }
    }
}
