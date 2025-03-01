namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    public class GetAllSalesRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }
    }
}
