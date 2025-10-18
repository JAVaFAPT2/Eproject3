using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleModel entity representing vehicle models available for purchase
    /// ModelNumber is the primary key (_id in MongoDB)
    /// </summary>
    public class VehicleModel
    {
        [BsonId]
        public string ModelNumber { get; private set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        [BsonElement("price")]
        [BsonRequired]
        public decimal Price { get; private set; }

        [BsonElement("description")]
        [BsonRequired]
        public string Description { get; private set; } = string.Empty;

        // Hierarchy
        [BsonElement("parentId")]
        public string? ParentId { get; private set; }

        [BsonElement("level")]
        public int Level { get; private set; } = 1; // 1 or 2

        // Slug for level-2 variants
        [BsonElement("slug")]
        public string? Slug { get; private set; }

        // Primary photo url for this model (usually first upload)
        [BsonElement("photo")]
        public string? Photo { get; private set; }

        // Soft delete timestamp
        [BsonElement("deletedAt")]
        public DateTimeOffset? DeletedAt { get; private set; }

    // Parameterless constructor for MongoDB deserialization
    public VehicleModel() { }

    public VehicleModel(string modelNumber, string name, decimal price, string description, string? parentId = null, int level = 1, string? slug = null, string? photo = null)
        {
            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty", nameof(description));

            ModelNumber = modelNumber;
            Name = name;
            Price = price;
            Description = description;
            ParentId = parentId;
            Level = level;
            Slug = slug;
            Photo = photo;
        }

    // Domain methods
        public void UpdateModel(string name, decimal price, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty", nameof(description));

            Name = name;
            Price = price;
            Description = description;
        }

        public void SetHierarchy(string? parentId, int level)
        {
            ParentId = parentId;
            Level = level;
        }

        public void SetSlug(string? slug)
        {
            Slug = slug;
        }

        public void SetPhoto(string? photoUrl)
        {
            Photo = photoUrl;
        }

        public void MarkDeleted()
        {
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
