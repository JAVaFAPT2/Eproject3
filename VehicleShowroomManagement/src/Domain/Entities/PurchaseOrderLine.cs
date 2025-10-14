using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrderLine entity for line items in a purchase order
    /// </summary>
    public class PurchaseOrderLine
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("poId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string POId { get; private set; } = string.Empty;

        [BsonElement("modelId")]
        [BsonRequired]
        public string ModelId { get; private set; } = string.Empty;

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; private set; }

        [BsonElement("pricePerUnit")]
        [BsonRequired]
        public decimal PricePerUnit { get; private set; }

        // Internal constructor for MongoDB
        internal PurchaseOrderLine() { }

        [BsonConstructor]
        public PurchaseOrderLine(string poId, string modelId, int quantity, decimal pricePerUnit)
        {
            if (string.IsNullOrWhiteSpace(poId))
                throw new ArgumentException("PO ID cannot be null or empty", nameof(poId));

            if (string.IsNullOrWhiteSpace(modelId))
                throw new ArgumentException("Model ID cannot be null or empty", nameof(modelId));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (pricePerUnit < 0)
                throw new ArgumentException("Price per unit cannot be negative", nameof(pricePerUnit));

            POId = poId;
            ModelId = modelId;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
        }

        // Domain methods
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            Quantity = quantity;
        }

        public void UpdatePricePerUnit(decimal pricePerUnit)
        {
            if (pricePerUnit < 0)
                throw new ArgumentException("Price per unit cannot be negative", nameof(pricePerUnit));

            PricePerUnit = pricePerUnit;
        }

        // Computed properties
        public decimal LineTotal => Quantity * PricePerUnit;
    }
}
