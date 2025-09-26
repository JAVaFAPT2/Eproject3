using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrder entity representing vehicle sales orders
    /// Central entity for managing vehicle sales transactions
    /// </summary>
    public class SalesOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int SalesPersonId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Draft"; // Draft, Confirmed, Invoiced, Completed, Cancelled

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("SalesPersonId")]
        public virtual User SalesPerson { get; set; } = null!;

        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        // Domain Methods
        public void ConfirmOrder()
        {
            Status = "Confirmed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsInvoiced()
        {
            Status = "Invoiced";
            UpdatedAt = DateTime.UtcNow;
        }

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

        public void UpdateTotalAmount(decimal amount)
        {
            TotalAmount = amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeModified()
        {
            return Status == "Draft" || Status == "Confirmed";
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
}