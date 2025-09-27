using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrder entity representing vehicle sales orders
    /// Central entity for managing vehicle sales transactions
    /// </summary>
    public class SalesOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("employeeId")]
        [BsonRequired]
        public string EmployeeId { get; set; } = string.Empty;

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [BsonElement("salePrice")]
        [BsonRequired]
        public decimal SalePrice { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Completed, Cancelled

        [BsonElement("dataSheetOutput")]
        public SalesOrderOutput? DataSheetOutput { get; set; }

        [BsonElement("confirmationOutput")]
        public SalesOrderOutput? ConfirmationOutput { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void MarkAsCompleted()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSalePrice(decimal salePrice)
        {
            SalePrice = salePrice;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDataSheetOutput(string format, DateTime generatedAt, string url)
        {
            DataSheetOutput = new SalesOrderOutput
            {
                Format = format,
                GeneratedAt = generatedAt,
                Url = url
            };
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetConfirmationOutput(string format, DateTime generatedAt, string url)
        {
            ConfirmationOutput = new SalesOrderOutput
            {
                Format = format,
                GeneratedAt = generatedAt,
                Url = url
            };
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeCancelled()
        {
            return Status != "Completed" && Status != "Cancelled";
        }

        public bool IsConfirmed()
        {
            return Status == "Confirmed";
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

    /// <summary>
    /// Sales order output information embedded in SalesOrder entity
    /// </summary>
    public class SalesOrderOutput
    {
        [BsonElement("format")]
        [BsonRequired]
        public string Format { get; set; } = string.Empty;

        [BsonElement("generatedAt")]
        [BsonRequired]
        public DateTime GeneratedAt { get; set; }

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; set; } = string.Empty;
    }
}