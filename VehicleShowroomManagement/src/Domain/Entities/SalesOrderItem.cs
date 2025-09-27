using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrderItem entity representing individual items in a sales order
    /// Each item represents a vehicle being sold
    /// </summary>
    public class SalesOrderItem 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("unitPrice")]
        [BsonRequired]
        public decimal UnitPrice { get; set; }

        [BsonElement("discount")]
        public decimal Discount { get; set; } = 0;

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; set; } = 1;

        [BsonElement("lineTotal")]
        public decimal LineTotal { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void CalculateLineTotal()
        {
            LineTotal = UnitPrice - Discount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ApplyDiscount(decimal discountAmount)
        {
            Discount = discountAmount;
            CalculateLineTotal();
        }

        public void UpdatePrice(decimal unitPrice)
        {
            UnitPrice = unitPrice;
            CalculateLineTotal();
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