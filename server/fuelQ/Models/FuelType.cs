﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace fuelQ.Models
{
    [BsonIgnoreExtraElements]
    public class FuelType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;
    }
}
