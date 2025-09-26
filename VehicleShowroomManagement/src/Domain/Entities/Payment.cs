using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Payment entity representing payments made against invoices
    /// </summary>
    public class Payment : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, CreditCard, BankTransfer, Check

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? ReferenceNumber { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Completed"; // Completed, Pending, Failed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; } = null!;

        // Domain Methods
        public void MarkAsCompleted()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPending()
        {
            Status = "Pending";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            Status = "Failed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsCompleted()
        {
            return Status == "Completed";
        }

        public bool IsPending()
        {
            return Status == "Pending";
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