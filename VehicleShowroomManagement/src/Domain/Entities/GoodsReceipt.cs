using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// GoodsReceipt entity representing vehicle goods receipt
    /// Manages the process of receiving vehicles with unique identification numbers
    /// </summary>
    public class GoodsReceipt : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("receiptNumber")]
        [BsonRequired]
        public string ReceiptNumber { get; set; } = string.Empty;

        [BsonElement("purchaseOrderId")]
        [BsonRequired]
        public string PurchaseOrderId { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("vin")]
        [BsonRequired]
        public string VIN { get; set; } = string.Empty; // Vehicle Identification Number

        [BsonElement("externalId")]
        public string? ExternalId { get; set; } // External number from vehicle manufacturer

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonElement("brand")]
        [BsonRequired]
        public string Brand { get; set; } = string.Empty;

        [BsonElement("price")]
        [BsonRequired]
        public decimal Price { get; set; }

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; set; } = 1;

        [BsonElement("status")]
        public string Status { get; set; } = "Received"; // Received, Inspected, Accepted, Rejected

        [BsonElement("receiptDate")]
        public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;

        [BsonElement("inspectedDate")]
        public DateTime? InspectedDate { get; set; }

        [BsonElement("inspectedBy")]
        public string? InspectedBy { get; set; }

        [BsonElement("inspectionNotes")]
        public string? InspectionNotes { get; set; }

        [BsonElement("condition")]
        public string Condition { get; set; } = "New"; // New, Used, Damaged, Refurbished

        [BsonElement("mileage")]
        public int Mileage { get; set; } = 0;

        [BsonElement("color")]
        public string? Color { get; set; }

        [BsonElement("year")]
        public int? Year { get; set; }

        [BsonElement("supplierId")]
        public string? SupplierId { get; set; }

        [BsonElement("supplierName")]
        public string? SupplierName { get; set; }

        [BsonElement("deliveryNote")]
        public string? DeliveryNote { get; set; }

        [BsonElement("receivedBy")]
        [BsonRequired]
        public string ReceivedBy { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void MarkAsInspected(string inspectedBy, string? notes = null)
        {
            Status = "Inspected";
            InspectedDate = DateTime.UtcNow;
            InspectedBy = inspectedBy;
            InspectionNotes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AcceptReceipt()
        {
            Status = "Accepted";
            UpdatedAt = DateTime.UtcNow;
        }

        public void RejectReceipt(string reason)
        {
            Status = "Rejected";
            InspectionNotes = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeInspected()
        {
            return Status == "Received";
        }

        public bool CanBeAccepted()
        {
            return Status == "Inspected";
        }

        public bool CanBeRejected()
        {
            return Status == "Inspected";
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
