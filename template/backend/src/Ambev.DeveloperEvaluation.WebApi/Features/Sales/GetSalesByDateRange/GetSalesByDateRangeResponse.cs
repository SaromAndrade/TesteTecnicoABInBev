using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeResponse
    {
        public List<Sale> Sales { get; set; }
        public int TotalItems { get; set; }
    }
}
