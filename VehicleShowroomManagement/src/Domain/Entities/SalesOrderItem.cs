using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrderItem entity representing individual items in a sales order
    /// Each item represents a vehicle being sold
    /// </summary>
    public class SalesOrderItem : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderItemId { get; set; }

        [Required]
        public int SalesOrderId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Discount { get; set; } = 0;

        [Column(TypeName = "decimal(12,2)")]
        public decimal LineTotal { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; } = null!;

        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        // Domain Methods
        public void CalculateLineTotal()
        {
            LineTotal = (UnitPrice * Quantity) - Discount;
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