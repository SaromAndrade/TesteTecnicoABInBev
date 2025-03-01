using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerResponse
    {
        public List<Sale> Sales { get; set; }
        public int TotalItems { get; set; }
    }
}
