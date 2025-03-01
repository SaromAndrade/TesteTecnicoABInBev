using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        public string CustomerId { get; set; }
        public List<SaleProductDto> Products { get; set; } = new();
        public bool IsCancelled { get; set; }
    }
}
