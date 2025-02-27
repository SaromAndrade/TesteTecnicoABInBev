using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("rating")]
        public Rating Rating { get; set; }
    }
    public class Rating
    {
        [BsonElement("rate")]
        public double Rate { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }
    }
}
