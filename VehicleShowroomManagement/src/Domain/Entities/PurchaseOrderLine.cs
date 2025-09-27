using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Purchase order line representing a line in a purchase order
    /// </summary>
    public class PurchaseOrderLine 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("poLineId")]
        [BsonRequired]
        public string PoLineId { get; set; } = string.Empty;

        [BsonElement("poId")]
        [BsonRequired]
        public string PoId { get; set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; set; }

        [BsonElement("pricePerUnit")]
        [BsonRequired]
        public decimal PricePerUnit { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Computed Properties
        [BsonIgnore]
        public decimal LineTotal => PricePerUnit * Quantity;

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal pricePerUnit)
        {
            PricePerUnit = pricePerUnit;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

