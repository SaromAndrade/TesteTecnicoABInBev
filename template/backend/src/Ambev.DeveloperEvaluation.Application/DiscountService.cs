using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain
{
    public class DiscountService
    {
        private readonly List<IDiscountStrategy> _discountStrategies;

        public DiscountService()
        {
            _discountStrategies = new List<IDiscountStrategy>
            {
                new NoDiscountStrategy(),
                new TenPercentDiscountStrategy(),
                new TwentyPercentDiscountStrategy()
            };
        }
        public decimal GetDiscount(int quantity, decimal unitPrice)
        {
            if (quantity >= 10 && quantity <= 20) return _discountStrategies[2].CalculateDiscount(quantity, unitPrice);
            if (quantity > 4) return _discountStrategies[1].CalculateDiscount(quantity, unitPrice);

            return 0;
        }
    }
}
