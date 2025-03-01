using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesResult
    {
        public List<Sale> Sales { get; set; }
        public int TotalItems { get; set; }
    }
}
