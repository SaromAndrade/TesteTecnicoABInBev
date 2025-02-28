using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Counter
    {
        [BsonId]
        public string Id { get; set; }
        public int SequenceValue { get; set; }
    }
}
