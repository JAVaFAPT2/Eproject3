using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehiclePhoto entity for vehicle images
    /// </summary>
    public class VehiclePhoto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; private set; } = string.Empty;

        [BsonElement("vehicleModelId")]
        public string? VehicleModelId { get; private set; }

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; private set; } = string.Empty;

        [BsonElement("displayOrder")]
        public int DisplayOrder { get; private set; }

        [BsonElement("caption")]
        public string? Caption { get; private set; }

        // Internal constructor for MongoDB
        internal VehiclePhoto() { }

        [BsonConstructor]
        public VehiclePhoto(string vehicleId, string? vehicleModelId, string url, int displayOrder = 0, string? caption = null)
        {
            if (string.IsNullOrWhiteSpace(vehicleId) && string.IsNullOrWhiteSpace(vehicleModelId))
                throw new ArgumentException("Either Vehicle ID or Vehicle Model ID must be provided");

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            VehicleId = vehicleId;
            VehicleModelId = vehicleModelId;
            Url = url;
            DisplayOrder = displayOrder;
            Caption = caption;
        }

        // Domain methods
        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        public void UpdateCaption(string? caption)
        {
            Caption = caption;
        }

        public void UpdateUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            Url = url;
        }
    }
}

