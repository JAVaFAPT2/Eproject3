using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Supplier entity representing vehicle manufacturers and suppliers
    /// </summary>
    public class Supplier : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? ContactPerson { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; }

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string? Phone { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string? Address { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? City { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? State { get; set; }

        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string? ZipCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

        // Computed Properties
        [NotMapped]
        public Address? SupplierAddress
        {
            get
            {
                if (string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State))
                    return null;

                return new Address(Address, City, State, ZipCode);
            }
        }

        // Domain Methods
        public void UpdateSupplier(string companyName, string? contactPerson, string? email, string? phone)
        {
            CompanyName = companyName;
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(string? address, string? city, string? state, string? zipCode)
        {
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
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