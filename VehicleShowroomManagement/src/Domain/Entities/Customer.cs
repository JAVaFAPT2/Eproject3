using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Customer entity representing customers who purchase vehicles
    /// </summary>
    public class Customer : IEntity, IAuditableEntity, ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

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
        public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public Address? CustomerAddress
        {
            get
            {
                if (string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State))
                    return null;

                return new Address(Address, City, State, ZipCode);
            }
        }

        // Domain Methods
        public void UpdateProfile(string firstName, string lastName, string email, string? phone)
        {
            FirstName = firstName;
            LastName = lastName;
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