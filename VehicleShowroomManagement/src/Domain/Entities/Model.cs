using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Model entity representing specific vehicle models under a brand
    /// </summary>
    public class Model : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string ModelName { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? EngineType { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? Transmission { get; set; }

        [StringLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public string? FuelType { get; set; }

        public int? SeatingCapacity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        // Domain Methods
        public void UpdateModel(string modelName, string? engineType, string? transmission, string? fuelType, int? seatingCapacity)
        {
            ModelName = modelName;
            EngineType = engineType;
            Transmission = transmission;
            FuelType = fuelType;
            SeatingCapacity = seatingCapacity;
            UpdatedAt = DateTime.UtcNow;
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