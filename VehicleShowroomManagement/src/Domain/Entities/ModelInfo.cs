using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Model information entity
    /// </summary>
    public class ModelInfo
    {
        [BsonElement("modelId")]
        [BsonRequired]
        public string ModelId { get; set; } = string.Empty;

        [BsonElement("modelName")]
        [BsonRequired]
        public string ModelName { get; set; } = string.Empty;

        [BsonElement("brandId")]
        [BsonRequired]
        public string BrandId { get; set; } = string.Empty;

        [BsonElement("brand")]
        public BrandInfo? Brand { get; set; }

        [BsonElement("engineType")]
        public string? EngineType { get; set; }

        [BsonElement("transmission")]
        public string? Transmission { get; set; }

        [BsonElement("fuelType")]
        public string? FuelType { get; set; }

        [BsonElement("seatingCapacity")]
        public int? SeatingCapacity { get; set; }
    }
}
