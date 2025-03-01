using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleProduct
    {
        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("unitPrice")]
        public decimal UnitPrice { get; set; }

        [BsonElement("totalItemAmount")]
        public decimal TotalItemAmount { get; set; }

        [BsonElement("discountAmount")]
        public decimal DiscountAmount { get; set; }

        [BsonElement("finalItemAmount")]
        public decimal FinalItemAmount { get; set; }
    }
}
