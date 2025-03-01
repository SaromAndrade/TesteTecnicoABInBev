using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("saleNumber")]
        public int SaleNumber { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("customer")]
        public string CustomerId { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("branchId")]
        public int BranchId { get; set; }

        [BsonElement("products")]
        public List<SaleProduct> Products { get; set; } = new List<SaleProduct>();

        [BsonElement("isCancelled")]
        public bool IsCancelled { get; set; }

        [BsonElement("finalAmount")]
        public decimal FinalAmount { get; set; }

        [BsonElement("DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        public void CancelSale()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            IsCancelled = true;
        }
    }
}
