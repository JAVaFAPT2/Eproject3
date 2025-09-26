using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Brand information entity
    /// </summary>
    public class BrandInfo
    {
        [BsonElement("brandId")]
        [BsonRequired]
        public string BrandId { get; set; } = string.Empty;

        [BsonElement("brandName")]
        [BsonRequired]
        public string BrandName { get; set; } = string.Empty;

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("logoUrl")]
        public string? LogoUrl { get; set; }
    }
}
