using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrderItem entity representing individual items in a purchase order
    /// Each item represents a vehicle being purchased
    /// </summary>
    public class PurchaseOrderItem : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("PurchaseId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        // Domain Methods
        public void CalculateTotalAmount()
        {
            TotalAmount = UnitPrice * Quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal unitPrice)
        {
            UnitPrice = unitPrice;
            CalculateTotalAmount();
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
            CalculateTotalAmount();
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