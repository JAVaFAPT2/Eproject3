using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Employee entities
    /// </summary>
    public class EmployeeDto
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Additional properties for reporting
        public int TotalSales { get; set; }

        public decimal TotalRevenue { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for Employee Profile
    /// </summary>
    public class EmployeeProfileDto
    {
        public string EmployeeId { get; set; } = string.Empty;

        public string EmployeeNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
