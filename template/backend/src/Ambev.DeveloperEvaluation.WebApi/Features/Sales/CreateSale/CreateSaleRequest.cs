using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string CustomerId { get; set; }
        public int BranchId { get; set; }
        public List<SaleProductDto> Products { get; set; }
    }
}
