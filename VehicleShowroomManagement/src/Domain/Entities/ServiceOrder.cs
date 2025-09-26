using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// ServiceOrder entity representing vehicle service orders
    /// Manages pre-delivery services and maintenance
    /// </summary>
    public class ServiceOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceOrderId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalCost { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<ServiceOrderItem> ServiceOrderItems { get; set; } = new List<ServiceOrderItem>();

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

        public void UpdateTotalCost(decimal cost)
        {
            TotalCost = cost;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newServiceDate)
        {
            ServiceDate = newServiceDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeStarted()
        {
            return Status == "Scheduled";
        }

        public bool CanBeCompleted()
        {
            return Status == "InProgress";
        }

        public bool CanBeCancelled()
        {
            return Status != "Completed";
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