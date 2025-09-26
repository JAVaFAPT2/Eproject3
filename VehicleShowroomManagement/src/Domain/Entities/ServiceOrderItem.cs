using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// ServiceOrderItem entity representing individual service items
    /// Each item represents a specific service performed on a vehicle
    /// </summary>
    public class ServiceOrderItem : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceOrderItemId { get; set; }

        [Required]
        public int ServiceOrderId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string ServiceType { get; set; } = string.Empty; // PreDelivery, Maintenance, Repair

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Cost { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; } = null!;

        // Domain Methods
        public void StartService()
        {
            Status = "InProgress";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteService()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelService()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCost(decimal cost)
        {
            Cost = cost;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeStarted()
        {
            return Status == "Pending";
        }

        public bool CanBeCompleted()
        {
            return Status == "InProgress";
        }

        public bool IsCompleted()
        {
            return Status == "Completed";
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