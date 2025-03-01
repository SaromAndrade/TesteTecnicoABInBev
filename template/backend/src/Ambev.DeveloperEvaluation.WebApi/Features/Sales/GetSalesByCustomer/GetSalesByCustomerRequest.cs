namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerRequest
    {
        public string CustomerId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }
    }
}
