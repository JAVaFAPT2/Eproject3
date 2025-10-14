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

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; private set; } = string.Empty;

        [BsonElement("modelId")]
        [BsonRequired]
        public string ModelId { get; private set; } = string.Empty;

        [BsonElement("displayOrder")]
        public int DisplayOrder { get; private set; }

        [BsonElement("caption")]
        public string? Caption { get; private set; }

        // Internal constructor for MongoDB
        internal VehiclePhoto() { }

        [BsonConstructor]
        public VehiclePhoto(string modelId, string url, int displayOrder = 0, string? caption = null)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                throw new ArgumentException("ModelId cannot be null or empty", nameof(modelId));
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url cannot be null or empty", nameof(url));

            ModelId = modelId;
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
                throw new ArgumentException("Url cannot be null or empty", nameof(url));
            Url = url;
        }

        public void UpdateModelId(string modelId)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                throw new ArgumentException("ModelId cannot be null or empty", nameof(modelId));

            ModelId = modelId;
        }
    }
}

