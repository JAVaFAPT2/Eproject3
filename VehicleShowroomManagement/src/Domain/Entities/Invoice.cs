using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Invoice entity representing billing documents for sales orders
    /// </summary>
    public class Invoice : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        public int SalesOrderId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(12,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Unpaid"; // Unpaid, Paid, PartiallyPaid, Overdue, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Domain Methods
        public void CalculateTotals(decimal subtotal, decimal taxRate)
        {
            Subtotal = subtotal;
            TaxAmount = subtotal * (taxRate / 100);
            TotalAmount = Subtotal + TaxAmount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPartiallyPaid()
        {
            Status = "PartiallyPaid";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsOverdue()
        {
            Status = "Overdue";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsPaid()
        {
            return Status == "Paid";
        }

        public bool IsUnpaid()
        {
            return Status == "Unpaid";
        }

        public bool IsOverdue()
        {
            return Status == "Overdue";
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