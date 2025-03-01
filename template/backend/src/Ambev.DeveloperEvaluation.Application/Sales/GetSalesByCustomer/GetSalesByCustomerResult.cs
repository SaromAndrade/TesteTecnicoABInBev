using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerResult
    {
        public List<Sale> Sales { get; set; }
        public int TotalItems { get; set; }
    }
}
