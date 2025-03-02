using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale should be marked as cancelled when CancelSale is called")]
        public void Given_ActiveSale_When_CancelSale_Then_SaleShouldBeCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(); // Gerar uma venda válida
            sale.IsCancelled = false; // Garantir que a venda está ativa

            // Act
            sale.CancelSale();

            // Assert
            Assert.True(sale.IsCancelled);
        }

        [Fact(DisplayName = "Cancelling an already cancelled sale should throw exception")]
        public void Given_CancelledSale_When_CancelSale_Then_ShouldThrowException()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.IsCancelled = true; // Venda já cancelada

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => sale.CancelSale());
            Assert.Equal("Sale is already cancelled.", exception.Message);
        }
    }
}
