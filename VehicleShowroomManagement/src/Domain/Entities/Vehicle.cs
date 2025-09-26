using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Vehicle entity representing vehicles in the showroom inventory
    /// Core entity for the vehicle showroom management system
    /// </summary>
    public class Vehicle : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(17)]
        [Column(TypeName = "nvarchar(17)")]
        public string VIN { get; set; } = string.Empty; // Vehicle Identification Number

        [Required]
        public int ModelId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Color { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        public int Mileage { get; set; } = 0;

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Available"; // Available, Sold, InService, Reserved

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ModelId")]
        public virtual Model Model { get; set; } = null!;

        public virtual ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();

        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();

        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

        // Domain Methods
        public void UpdateVehicleInfo(string color, int year, decimal price, int mileage)
        {
            Color = color;
            Year = year;
            Price = price;
            Mileage = mileage;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsSold()
        {
            Status = "Sold";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsAvailable()
        {
            Status = "Available";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsInService()
        {
            Status = "InService";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reserve()
        {
            Status = "Reserved";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsAvailableForSale()
        {
            return Status == "Available";
        }

        public bool CanBeServiced()
        {
            return Status != "InService";
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