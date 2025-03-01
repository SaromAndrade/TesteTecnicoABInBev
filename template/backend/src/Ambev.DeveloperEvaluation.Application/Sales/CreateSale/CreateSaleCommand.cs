using Ambev.DeveloperEvaluation.Application.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string CustomerId { get; set; }
        public int BranchId { get; set; }
        public List<SaleProductDto> Products { get; set; } = new();
    }
}
