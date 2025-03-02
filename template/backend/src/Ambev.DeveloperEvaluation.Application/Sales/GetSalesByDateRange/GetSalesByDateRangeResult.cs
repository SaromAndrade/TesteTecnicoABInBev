using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeResult
    {
        public List<Sale> Sales { get; set; }
        public int TotalItems { get; set; }
    }
}
