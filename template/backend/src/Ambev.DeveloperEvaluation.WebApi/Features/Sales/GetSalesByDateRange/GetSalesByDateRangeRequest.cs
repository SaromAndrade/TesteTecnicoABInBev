namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }
    }
}
