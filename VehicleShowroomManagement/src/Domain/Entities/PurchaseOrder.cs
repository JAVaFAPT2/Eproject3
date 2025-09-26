using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrder entity representing orders placed with suppliers
    /// Manages the procurement of vehicles from manufacturers
    /// </summary>
    public class PurchaseOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Delivered, Cancelled

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeliveryDate { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } = null!;

        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

        // Domain Methods
        public void ConfirmOrder()
        {
            Status = "Confirmed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsDelivered()
        {
            Status = "Delivered";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDeliveryDate(DateTime deliveryDate)
        {
            DeliveryDate = deliveryDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTotalAmount(decimal amount)
        {
            TotalAmount = amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeModified()
        {
            return Status == "Pending";
        }

        public bool CanBeCancelled()
        {
            return Status != "Delivered" && Status != "Cancelled";
        }

        public bool IsConfirmed()
        {
            return Status == "Confirmed";
        }

        public bool IsDelivered()
        {
            return Status == "Delivered";
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