using Ambev.DeveloperEvaluation.Application.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public List<SaleProductDto> Products { get; set; } = new();
        public bool IsCancelled { get; set; }
    }
}
