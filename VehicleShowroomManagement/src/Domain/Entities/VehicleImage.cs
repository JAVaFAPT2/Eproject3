using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleImage entity representing images associated with vehicles
    /// Manages vehicle photographs and image optimization
    /// </summary>
    public class VehicleImage : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string ImageType { get; set; } = "Exterior"; // Exterior, Interior, Engine, Other

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public int FileSize { get; set; } // in bytes

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string PublicId { get; set; } = string.Empty; // Cloudinary public ID

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string OriginalFileName { get; set; } = string.Empty;

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string ContentType { get; set; } = string.Empty;

        public bool IsPrimary { get; set; } = false; // Primary image for vehicle

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        // Domain Methods
        public void UpdateImage(string imageUrl, string imageType, string fileName, int fileSize)
        {
            ImageUrl = imageUrl;
            ImageType = imageType;
            FileName = fileName;
            FileSize = fileSize;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeImageType(string imageType)
        {
            ImageType = imageType;
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