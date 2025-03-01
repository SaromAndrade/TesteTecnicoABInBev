using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(int quantity, decimal unitPrice) => 0;
    }
    public class TenPercentDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            return (unitPrice * quantity) * 0.10m;
        }
    }
    public class TwentyPercentDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            return (unitPrice * quantity) * 0.20m;
        }
    }
}
