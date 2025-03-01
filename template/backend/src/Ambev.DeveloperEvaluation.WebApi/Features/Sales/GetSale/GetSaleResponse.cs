using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleResponse
    {
        public int Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; private set; }
        public string BranchId { get; set; }
        public List<SaleProduct> Products { get; set; }
        public bool IsCancelled { get; set; }
        public decimal FinalAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
    }
}
